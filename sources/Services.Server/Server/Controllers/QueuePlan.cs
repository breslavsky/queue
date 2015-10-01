using AutoMapper;
using Junte.Translation;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    public partial class ServerService
    {
        public async Task<DTO.ClientRequestPlan[]> GetOperatorClientRequestPlans()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    try
                    {
                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.ReadLock())
                        {
                            var clientRequestPlans = todayQueuePlan.GetOperatorPlan(queueOperator)
                                .ClientRequestPlans.OrderBy(p => p.StartTime);

                            return Mapper.Map<IEnumerable<ClientRequestPlan>, DTO.ClientRequestPlan[]>(clientRequestPlans);
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task<Dictionary<DTO.Operator, DTO.ClientRequestPlan>> GetCurrentClientRequestPlans()
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.ReadLock())
                        {
                            var plans = new Dictionary<Operator, ClientRequestPlan>();

                            foreach (var o in todayQueuePlan.OperatorsPlans)
                            {
                                plans.Add(o.Operator, o.CurrentClientRequestPlan);
                            }

                            return Mapper.Map<Dictionary<Operator, ClientRequestPlan>,
                                Dictionary<DTO.Operator, DTO.ClientRequestPlan>>(plans);
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task<DTO.ClientRequestPlan> GetCurrentClientRequestPlan()
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                var queueOperator = (Operator)currentUser;

                try
                {
                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.ReadLock())
                    {
                        var clientRequestPlan = todayQueuePlan.GetOperatorPlan(queueOperator)
                            .CurrentClientRequestPlan;

                        return Mapper.Map<ClientRequestPlan, DTO.ClientRequestPlan>(clientRequestPlan);
                    }
                }
                catch
                {
                    return null;
                }
            });
        }

        public async Task CallCurrentClient()
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    ClientRequest clientRequest;

                    try
                    {
                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.ReadLock())
                        {
                            var currentClientRequestPlan = todayQueuePlan.GetOperatorPlan(queueOperator)
                                .CurrentClientRequestPlan;
                            if (currentClientRequestPlan == null)
                            {
                                throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(queueOperator.GetId()), string.Format("Текущий запрос клиента не найден у оператора [{0}]", queueOperator));
                            }

                            clientRequest = session.Merge(currentClientRequestPlan.ClientRequest);
                        }

                        clientRequest.CallingLastTime = DateTime.Now.TimeOfDay;
                        session.Save(clientRequest);

                        var queueEvent = new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("Оператор [{0}] вызывает клиента", clientRequest.Operator)
                        };
                        session.Save(queueEvent);

                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            todayQueuePlan.Put(clientRequest);
                        }

                        QueueInstance.CallClient(clientRequest);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task UpdateCurrentClientRequest(ClientRequestState state)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    ClientRequest clientRequest;

                    var todayQueuePlan = QueueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.ReadLock())
                    {
                        var currentClientRequestPlan = todayQueuePlan.GetOperatorPlan(queueOperator)
                            .CurrentClientRequestPlan;
                        if (currentClientRequestPlan == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(queueOperator.GetId()), string.Format("Текущий запрос клиента не найден у оператора [{0}]", queueOperator));
                        }

                        clientRequest = session.Merge(currentClientRequestPlan.ClientRequest);
                    }

                    try
                    {
                        var service = clientRequest.Service;
                        var client = clientRequest.Client;

                        string message;

                        switch (clientRequest.State)
                        {
                            case ClientRequestState.Waiting:
                            case ClientRequestState.Redirected:
                            case ClientRequestState.Postponed:
                                switch (state)
                                {
                                    case ClientRequestState.Calling:
                                        message = string.Format("Клиент [{0}] вызывается к оператору [{1}] на рабочее место [{2}]",
                                            client, queueOperator, queueOperator.Workplace);
                                        clientRequest.Calling(queueOperator);
                                        break;

                                    default:
                                        throw new Exception(string.Format("Изменение состояния с [{0}] на [{1}] заявки клиента [{2}] не допустимо",
                                            clientRequest.State, state, clientRequest));
                                }
                                break;

                            case ClientRequestState.Calling:
                                switch (state)
                                {
                                    case ClientRequestState.Absence:
                                        if (clientRequest.ClientRecalls >= service.MaxClientRecalls)
                                        {
                                            message = string.Format("Клиент [{0}] не подошел к оператору [{1}]", client, queueOperator);
                                            clientRequest.Absence();
                                        }
                                        else
                                        {
                                            message = string.Format("Клиент [{0}] не подошел к оператору [{1}] вызов откладывается", client, queueOperator);
                                            clientRequest.Postpone(TimeSpan.FromMinutes(5));
                                            clientRequest.ClientRecalls++;
                                        }
                                        break;

                                    case ClientRequestState.Rendering:
                                        message = string.Format("Начало обслуживания клиента [{0}] у оператора [{1}]", client, queueOperator);
                                        var schedule = todayQueuePlan.GetServiceSchedule(service);
                                        clientRequest.Rendering(schedule.LiveClientInterval);
                                        break;

                                    default:
                                        throw new Exception(string.Format("Изменение состояния с [{0}] на [{1}] заявки клиента [{2}] не допустимо",
                                            clientRequest.State, state, clientRequest));
                                }
                                break;

                            case ClientRequestState.Rendering:
                                switch (state)
                                {
                                    case ClientRequestState.Rendered:

                                        if (!clientRequest.NextStep(session))
                                        {
                                            if (clientRequest.Service.IsUseType
                                                && clientRequest.ServiceType == ServiceType.None)
                                            {
                                                throw new Exception("Укажите тип услуги перед окончанием обслуживания");
                                            }

                                            clientRequest.Rendered();
                                            message = string.Format("Завершено обслуживание клиента [{0}] у оператора [{1}] с производительностью {2:00.00}%",
                                                client, queueOperator, clientRequest.Productivity);
                                        }
                                        else
                                        {
                                            message = string.Format("Запросу установлен следующий этап оказания услуги [{0}]", clientRequest.ServiceStep);
                                        }

                                        break;

                                    default:
                                        throw new Exception(string.Format("Изменение состояния с [{0}] на [{1}] заявки клиента [{2}] не допустимо",
                                            clientRequest.State, state, clientRequest));
                                }
                                break;

                            default:
                                throw new Exception(string.Format("Заявка клиента [{0}] находиться в не допустимом состоянии [{1}]",
                                    clientRequest, clientRequest.State));
                        }

                        clientRequest.Version++;
                        session.Save(clientRequest);

                        var queueEvent = new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = message
                        };
                        session.Save(queueEvent);

                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            todayQueuePlan.Put(clientRequest);
                            todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                        }

                        QueueInstance.ClientRequestUpdated(clientRequest);
                        QueueInstance.Event(queueEvent);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task RedirectToOperatorCurrentClientRequest(Guid redirectOperatorId)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    try
                    {
                        var clientRequest = session.Merge(QueueInstance.TodayQueuePlan
                            .GetOperatorPlan(queueOperator).CurrentClientRequestPlan.ClientRequest);

                        var targetOperator = session.Get<Operator>(redirectOperatorId);
                        if (targetOperator == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(redirectOperatorId),
                                string.Format("Оператор для передачи [{0}] не найден", redirectOperatorId));
                        }

                        if (targetOperator.Equals(queueOperator))
                        {
                            throw new FaultException("Не возможно передать запрос самому себе");
                        }

                        clientRequest.Redirect(targetOperator);
                        clientRequest.Version++;
                        session.Save(clientRequest);

                        var queueEvent = new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("Запрос клиента передан к оператору {0}", targetOperator)
                        };

                        session.Save(queueEvent);

                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            todayQueuePlan.Put(clientRequest);
                            todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                        }

                        QueueInstance.ClientRequestUpdated(clientRequest);
                        QueueInstance.Event(queueEvent);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task CallClientByRequestNumber(int number)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    try
                    {
                        var events = new List<ClientRequestEvent>();

                        ClientRequest currentClientRequest = null;

                        var currentClientRequestPlan = QueueInstance.TodayQueuePlan
                            .GetOperatorPlan(queueOperator).CurrentClientRequestPlan;

                        if (currentClientRequestPlan != null)
                        {
                            currentClientRequest = session.Merge(currentClientRequestPlan.ClientRequest);
                            switch (currentClientRequest.State)
                            {
                                case ClientRequestState.Calling:

                                    currentClientRequest.Return();

                                    currentClientRequest.Version++;
                                    session.Save(currentClientRequest);
                                    {
                                        var queueEvent = new ClientRequestEvent()
                                        {
                                            ClientRequest = currentClientRequest,
                                            Message = string.Format("Запрос клиента возвращен в очередь {0}", queueOperator)
                                        };
                                        session.Save(queueEvent);
                                        events.Add(queueEvent);
                                    }

                                    break;

                                case ClientRequestState.Rendering:

                                    if (!currentClientRequest.NextStep(session))
                                    {
                                        if (currentClientRequest.Service.IsUseType
                                            && currentClientRequest.ServiceType == ServiceType.None)
                                        {
                                            throw new Exception("Укажите тип услуги перед окончанием обслуживания");
                                        }

                                        currentClientRequest.Rendered();

                                        currentClientRequest.Version++;
                                        session.Save(currentClientRequest);
                                        {
                                            var queueEvent = new ClientRequestEvent()
                                            {
                                                ClientRequest = currentClientRequest,
                                                Message = string.Format("Завершено обслуживание клиента [{0}] у оператора [{1}] с производительностью {2:00.00}%",
                                                    currentClientRequest.Client, queueOperator, currentClientRequest.Productivity)
                                            };
                                            session.Save(queueEvent);
                                            events.Add(queueEvent);
                                        }
                                    }
                                    else
                                    {
                                        var queueEvent = new ClientRequestEvent()
                                        {
                                            ClientRequest = currentClientRequest,
                                            Message = string.Format("Запросу установлен следующий этап оказания услуги [{0}]", currentClientRequest.ServiceStep)
                                        };
                                        session.Save(queueEvent);
                                        events.Add(queueEvent);
                                    }

                                    break;
                            }
                        }

                        var clientRequest = session.CreateCriteria<ClientRequest>()
                            .Add(Restrictions.Eq("RequestDate", DateTime.Today))
                            .Add(Restrictions.Eq("Number", number))
                            .UniqueResult<ClientRequest>();
                        if (clientRequest == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(number),
                                string.Format("Запрос клиента [{0}] не найден", number));
                        }

                        if (clientRequest.IsClosed)
                        {
                            clientRequest.Restore();
                            {
                                var queueEvent = new ClientRequestEvent()
                                {
                                    ClientRequest = currentClientRequest,
                                    Message = string.Format("Запрос клиента [{0}] восстановлен", clientRequest)
                                };
                                session.Save(queueEvent);
                                events.Add(queueEvent);
                            }
                        }

                        clientRequest.Calling(queueOperator);

                        {
                            var queueEvent = new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("Запрос клиента вызван по номеру к оператору {0}", queueOperator)
                            };

                            session.Save(queueEvent);
                            QueueInstance.Event(queueEvent);
                        }

                        clientRequest.Version++;
                        session.Save(clientRequest);

                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            if (currentClientRequest != null)
                            {
                                todayQueuePlan.Put(currentClientRequest);
                                QueueInstance.ClientRequestUpdated(currentClientRequest);
                            }

                            todayQueuePlan.AddClientRequest(clientRequest);
                            todayQueuePlan.Build(DateTime.Now.TimeOfDay);

                            QueueInstance.ClientRequestUpdated(clientRequest);
                            QueueInstance.CallClient(clientRequest);
                        }

                        foreach (var e in events)
                        {
                            QueueInstance.Event(e);
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task ReturnCurrentClientRequest()
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    try
                    {
                        var clientRequest = session.Merge(QueueInstance.TodayQueuePlan
                            .GetOperatorPlan(queueOperator).CurrentClientRequestPlan.ClientRequest);

                        clientRequest.Return();
                        clientRequest.Version++;
                        session.Save(clientRequest);

                        var queueEvent = new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("Запрос клиента был возвращен в очередь оператором {0}", queueOperator)
                        };

                        session.Save(queueEvent);

                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            todayQueuePlan.Put(clientRequest);
                            todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                        }

                        QueueInstance.ClientRequestUpdated(clientRequest);
                        QueueInstance.Event(queueEvent);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task PostponeCurrentClientRequest(TimeSpan postponeTime)
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    try
                    {
                        ClientRequest clientRequest;

                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.ReadLock())
                        {
                            clientRequest = session.Merge(todayQueuePlan.GetOperatorPlan(queueOperator)
                                .CurrentClientRequestPlan.ClientRequest);
                        }

                        clientRequest.Postpone(postponeTime);
                        clientRequest.Version++;
                        session.Save(clientRequest);

                        var queueEvent = new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("Запрос клиента был отложен оператором {0} на {1} мин.", queueOperator, postponeTime.TotalMinutes)
                        };

                        session.Save(queueEvent);

                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            todayQueuePlan.Put(clientRequest);
                            todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                        }

                        QueueInstance.ClientRequestUpdated(clientRequest);
                        QueueInstance.Event(queueEvent);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task<DTO.ClientRequest> EditCurrentClientRequest(DTO.ClientRequest source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Operator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var queueOperator = (Operator)currentUser;

                    try
                    {
                        ClientRequest clientRequest;

                        var todayQueuePlan = QueueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.ReadLock())
                        {
                            var clientRequestPlan = todayQueuePlan.GetOperatorPlan(queueOperator)
                                .CurrentClientRequestPlan;
                            if (clientRequestPlan == null)
                            {
                                throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(queueOperator.GetId()),
                                    string.Format("Текущий запрос клиента не найден у оператора [{0}]", queueOperator));
                            }

                            clientRequest = session.Merge(clientRequestPlan.ClientRequest);
                        }

                        if (clientRequest.State != ClientRequestState.Rendering)
                        {
                            throw new FaultException("Нельзя изменить запрос клиент находящийся в данном статусе");
                        }

                        if (clientRequest.Rating != source.Rating)
                        {
                            clientRequest.Rating = source.Rating;

                            session.Save(new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("[{0}] установлена оценка качества обслуживания [{1}] для запроса клиента [{2}]",
                                    queueOperator, clientRequest.Rating, clientRequest)
                            });
                        }

                        if (clientRequest.Subjects != source.Subjects)
                        {
                            clientRequest.Subjects = source.Subjects;

                            session.Save(new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("[{0}] изменил количество запросов на [{1}] для запроса клиента [{2}]",
                                    queueOperator, clientRequest.Subjects, clientRequest)
                            });
                        }

                        if (source.Service != null)
                        {
                            Guid serviceId = source.Service.Id;

                            var service = session.Get<Service>(serviceId);
                            if (service == null)
                            {
                                throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                                    string.Format("Услуга [{0}] не найдена", serviceId));
                            }

                            if (clientRequest.Service == null
                                || !clientRequest.Service.Equals(service))
                            {
                                clientRequest.Service = service;
                                clientRequest.FirstStep(session);

                                session.Save(new ClientRequestEvent()
                                {
                                    ClientRequest = clientRequest,
                                    Message = string.Format("[{0}] изменил услугу на [{1}] для запроса клиента [{2}]",
                                        queueOperator, service, clientRequest)
                                });
                            }
                        }
                        else if (clientRequest.Service != null)
                        {
                            clientRequest.Service = null;
                        }

                        if (source.ServiceStep != null)
                        {
                            Guid serviceStepId = source.ServiceStep.Id;

                            var serviceStep = session.Get<ServiceStep>(serviceStepId);
                            if (serviceStep == null)
                            {
                                throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceStepId),
                                    string.Format("Этап услуги [{0}] не найден", serviceStepId));
                            }

                            if (clientRequest.ServiceStep == null
                                || !clientRequest.ServiceStep.Equals(serviceStep))
                            {
                                clientRequest.ServiceStep = serviceStep;

                                session.Save(new ClientRequestEvent()
                                {
                                    ClientRequest = clientRequest,
                                    Message = string.Format("[{0}] изменил этап услуги на [{1}] для запроса клиента [{2}]",
                                        queueOperator, serviceStep, clientRequest)
                                });
                            }
                        }
                        else if (clientRequest.ServiceStep != null)
                        {
                            clientRequest.ServiceStep = null;

                            session.Save(new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("[{0}] сбросил этап услуги для запроса клиента [{1}]", queueOperator, clientRequest)
                            });
                        }

                        if (!clientRequest.ServiceType.Equals(source.ServiceType))
                        {
                            clientRequest.ServiceType = source.ServiceType;

                            session.Save(new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("[{0}] изменил тип услуги на [{1}] для запроса клиента [{2}]", queueOperator,
                                    Translater.Enum(clientRequest.ServiceType), clientRequest)
                            });
                        }

                        clientRequest.Comment = source.Comment;

                        var error = clientRequest.Validate().FirstOrDefault();
                        if (error != null)
                        {
                            throw new FaultException(error.Message);
                        }

                        clientRequest.Version++;
                        session.Save(clientRequest);

                        using (var locker = todayQueuePlan.WriteLock())
                        {
                            transaction.Commit();

                            todayQueuePlan.Put(clientRequest);
                            todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                        }

                        return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task<DTO.QueuePlan> GetQueuePlan(DateTime planDate)
        {
            return await Task.Run(() =>
            {
                planDate = planDate.Date;

                QueuePlan queuePlan;

                if (planDate == DateTime.Today)
                {
                    queuePlan = QueueInstance.TodayQueuePlan;
                }
                else
                {
                    queuePlan = new QueuePlan();
                    queuePlan.Load(planDate);
                    queuePlan.Build();
                }

                using (var locker = queuePlan.ReadLock())
                {
                    return Mapper.Map<QueuePlan, DTO.QueuePlan>(queuePlan);
                }
            });
        }

        public async Task<DTO.ServiceFreeTime> GetServiceFreeTime(Guid serviceId, DateTime planDate, ClientRequestType requestType)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    planDate = planDate.Date;

                    QueuePlan queuePlan;

                    if (planDate == DateTime.Today)
                    {
                        queuePlan = QueueInstance.TodayQueuePlan;
                    }
                    else
                    {
                        queuePlan = new QueuePlan();
                        queuePlan.Load(planDate);
                        queuePlan.Build();
                    }

                    try
                    {
                        using (var locker = queuePlan.ReadLock())
                        {
                            var serviceFreeTime = queuePlan.GetServiceFreeTime(service, service.GetFirstStep(session), requestType);
                            return Mapper.Map<ServiceFreeTime, DTO.ServiceFreeTime>(serviceFreeTime);
                        }
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }
                }
            });
        }

        public async Task<DTO.Schedule> GetServiceCurrentSchedule(Guid serviceId, DateTime planDate)
        {
            return await Task.Run(() =>
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId), string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    planDate = planDate.Date;

                    QueuePlan queuePlan;

                    if (planDate == DateTime.Today)
                    {
                        queuePlan = QueueInstance.TodayQueuePlan;
                    }
                    else
                    {
                        queuePlan = new QueuePlan();
                        queuePlan.Load(planDate);
                        queuePlan.Build();
                    }

                    using (var locker = queuePlan.ReadLock())
                    {
                        var schedule = queuePlan.GetServiceSchedule(service);
                        return Mapper.Map<Schedule, DTO.Schedule>(schedule);
                    }
                }
            });
        }

        public async Task RefreshTodayQueuePlan()
        {
            await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.QueuePlan);

                var queuePlan = QueueInstance.TodayQueuePlan;

                using (var locker = queuePlan.WriteLock())
                {
                    queuePlan.Refresh();
                    queuePlan.Build(DateTime.Now.TimeOfDay);
                }
            });
        }
    }
}