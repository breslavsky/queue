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
        public async Task<DTO.ClientRequest[]> FindClientRequests(int startIndex, int maxResults, DTO.ClientRequestFilter filter)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.ClientsRequests);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var criteria = session.CreateCriteria<ClientRequest>()
                        .AddOrder(Order.Desc("RequestDate"))
                        .AddOrder(Order.Asc("RequestTime"))
                        .SetFirstResult(startIndex)
                        .SetMaxResults(maxResults);

                    if (filter.IsRequestDate)
                    {
                        criteria.Add(Restrictions.Eq("RequestDate", filter.RequestDate));
                    }

                    if (filter.IsOperator)
                    {
                        criteria.Add(Restrictions.Eq("Operator.Id", filter.OperatorId));
                    }

                    if (filter.IsService)
                    {
                        criteria.Add(Restrictions.Eq("Service.Id", filter.ServiceId));
                    }

                    if (filter.IsClient)
                    {
                        criteria.Add(Restrictions.Eq("Client.Id", filter.ClientId));
                    }

                    if (filter.IsState)
                    {
                        criteria.Add(Restrictions.Eq("State", filter.State));
                    }

                    string query = filter.Query;

                    if (!string.IsNullOrEmpty(query))
                    {
                        try
                        {
                            int number = int.Parse(query);
                            criteria.Add(Restrictions.Eq("Number", number));
                        }
                        catch
                        {
                            var subQuery = DetachedCriteria.For<Client>("client")
                                .SetProjection(Projections.Id())
                                .Add(Restrictions.Disjunction()
                                        .Add(Restrictions.InsensitiveLike("Surname", query, MatchMode.Anywhere))
                                        .Add(Restrictions.InsensitiveLike("Name", query, MatchMode.Anywhere))
                                        .Add(Restrictions.InsensitiveLike("Patronymic", query, MatchMode.Anywhere))
                                        .Add(Restrictions.InsensitiveLike("Mobile", query, MatchMode.Anywhere))
                                );
                            criteria.Add(Subqueries.PropertyIn("Client", subQuery));
                        }
                    }

                    var requests = criteria.List<ClientRequest>();

                    return Mapper.Map<IList<ClientRequest>, DTO.ClientRequest[]>(requests);
                }
            });
        }

        public async Task<DTO.ClientRequest> GetClientRequest(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.All);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId), string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }

        public async Task<DTO.ClientRequestEvent[]> GetClientRequestEvents(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.All);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId), string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    var events = session.CreateCriteria<ClientRequestEvent>()
                        .Add(Restrictions.Eq("ClientRequest", clientRequest))
                        .AddOrder(Order.Desc("CreateDate"))
                        .List<ClientRequestEvent>();

                    return Mapper.Map<IList<ClientRequestEvent>, DTO.ClientRequestEvent[]>(events);
                }
            });
        }

        public async Task<DTO.ClientRequest> AddEarlyClientRequest(Guid clientId, Guid serviceId, DateTime requestDate, TimeSpan requestTime, Dictionary<Guid, object> parameters, int subjects)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.ClientsRequests);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Client client = null;

                    if (clientId != Guid.Empty)
                    {
                        client = session.Get<Client>(clientId);
                        if (client == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientId),
                                string.Format("Клиент [{0}] не найден", clientId));
                        }
                    }

                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                            string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    if (!service.IsActive)
                    {
                        throw new FaultException("Выбранная услуга не активна");
                    }

                    requestDate = requestDate.Date;
                    var isRequestDateToday = requestDate == DateTime.Today;

                    QueuePlan queuePlan;

                    if (isRequestDateToday)
                    {
                        queuePlan = queueInstance.TodayQueuePlan;
                    }
                    else
                    {
                        if (requestDate - DateTime.Today > TimeSpan.FromDays(service.MaxEarlyDays))
                        {
                            throw new FaultException(string.Format("По указанной услуге запись возможна не более чем за {0} дней", service.MaxEarlyDays));
                        }

                        queuePlan = new QueuePlan();
                        queuePlan.Load(requestDate);
                        queuePlan.Build();
                    }

                    var clientRequest = new ClientRequest()
                    {
                        Client = client,
                        Service = service,
                        ServiceStep = service.GetFirstStep(session),
                        Subjects = subjects
                    };

                    using (var locker = queuePlan.ReadLock())
                    {
                        var schedule = queuePlan.GetServiceSchedule(service);

                        int count = session.CreateCriteria<ClientRequest>()
                            .Add(Restrictions.Eq("RequestDate", requestDate))
                            .Add(Restrictions.Eq("Client", client))
                            .Add(Restrictions.Eq("Service", service))
                            .SetProjection(Projections.RowCount())
                            .UniqueResult<int>();

                        if (count >= schedule.MaxClientRequests)
                        {
                            throw new FaultException("Достигнуто максимальное кол-во запрос на текущий день");
                        }

                        try
                        {
                            var freeTime = queuePlan.GetServiceFreeTime(service, clientRequest.ServiceStep,
                                ClientRequestType.Early, subjects);
                            var timeIntervals = freeTime.TimeIntervals;
                            if (timeIntervals.Length == 0)
                            {
                                throw new FaultException("Нет свободного времени для оказания услуги");
                            }

                            if (!timeIntervals.Contains(requestTime))
                            {
                                throw new FaultException("Выбрано недопустимое время");
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new FaultException(exception.Message);
                        }

                        if (subjects < 1)
                        {
                            throw new FaultException("Количество объектов должно быть больше 0");
                        }
                    }

                    logger.Info("Добавление нового запроса в план очереди");

                    using (var locker = queuePlan.WriteLock())
                    {
                        clientRequest.Number = ++queuePlan.LastNumber;
                    }

                    clientRequest.RequestDate = requestDate;
                    clientRequest.RequestTime = requestTime;
                    clientRequest.Type = ClientRequestType.Early;

                    var error = clientRequest.Validate().FirstOrDefault();
                    if (error != null)
                    {
                        throw new FaultException(error.Message);
                    }

                    session.Save(clientRequest);

                    var queueEvent = new ClientRequestEvent()
                    {
                        ClientRequest = clientRequest,
                        Message = string.Format("[{0}] добавил новый запрос клиента по предварительной записи на дату [{1}] и время [{2}] [{3}]", currentUser, requestDate.ToShortDateString(), requestTime, clientRequest)
                    };

                    session.Save(queueEvent);

                    var defaultConfig = session.Get<DefaultConfig>(ConfigType.Default);
                    if (defaultConfig.IsDebug)
                    {
                        session.Save(new QueuePlanReport()
                        {
                            ClientRequest = clientRequest,
                            Report = string.Join(Environment.NewLine, queuePlan.Report)
                        });
                    }

                    if (isRequestDateToday)
                    {
                        using (var locker = queuePlan.WriteLock())
                        {
                            transaction.Commit();

                            queuePlan.AddClientRequest(clientRequest);
                            queuePlan.Build(DateTime.Now.TimeOfDay);
                        }
                    }
                    else
                    {
                        transaction.Commit();
                    }

                    queueInstance.Event(queueEvent);

                    logger.Info("Запрос добавлен в план очереди");

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }

        public async Task<DTO.ClientRequest> AddLiveClientRequest(Guid clientId, Guid serviceId, bool isPriority, Dictionary<Guid, object> parameters, int subjects)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.ClientsRequests);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Client client = null;

                    if (clientId != Guid.Empty)
                    {
                        client = session.Get<Client>(clientId);
                        if (client == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientId),
                                string.Format("Клиент [{0}] не найден", clientId));
                        }
                    }

                    var service = session.Get<Service>(serviceId);
                    if (service == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(serviceId),
                            string.Format("Услуга [{0}] не найдена", serviceId));
                    }

                    if (!service.IsActive)
                    {
                        throw new FaultException("Выбранная услуга не активна");
                    }

                    if (service.ClientRequire && client == null)
                    {
                        throw new FaultException("Выбранная услуга требует наличие клиента");
                    }

                    var clientRequest = new ClientRequest()
                    {
                        Client = client,
                        Service = service,
                        ServiceStep = service.GetFirstStep(session),
                        Subjects = subjects,
                        IsPriority = client != null && client.VIP
                    };

                    var currentTime = DateTime.Now.TimeOfDay;

                    var todayQueuePlan = queueInstance.TodayQueuePlan;

                    using (var locker = todayQueuePlan.ReadLock())
                    {
                        var schedule = todayQueuePlan.GetServiceSchedule(service);

                        int count = session.CreateCriteria<ClientRequest>()
                            .Add(Restrictions.Eq("RequestDate", DateTime.Today))
                            .Add(Restrictions.Eq("Client", client))
                            .Add(Restrictions.Eq("Service", service))
                            .SetProjection(Projections.RowCount())
                            .UniqueResult<int>();

                        if (count >= schedule.MaxClientRequests)
                        {
                            throw new FaultException("Достигнуто максимальное кол-во запрос на текущий день");
                        }

#if !DEBUG
                        if (currentTime < schedule.StartTime)
                        {
                            throw new FaultException(string.Format("Время начала оказания услуги наступит в {0}", schedule.StartTime));
                        }
#endif

                        try
                        {
                            var freeTime = todayQueuePlan.GetServiceFreeTime(service, clientRequest.ServiceStep,
                                ClientRequestType.Live, service.IsPlanSubjects ? subjects : 1);
                            var timeIntervals = freeTime.TimeIntervals;
                            if (timeIntervals.Length == 0)
                            {
                                throw new FaultException("Нет свободного времени для оказания услуги");
                            }
                        }
                        catch (Exception exception)
                        {
                            throw new FaultException(exception.Message);
                        }
                    }

                    logger.Info("Добавление нового запроса в план очереди");

                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        clientRequest.Number = ++todayQueuePlan.LastNumber;
                    }

                    clientRequest.RequestDate = DateTime.Today;

                    clientRequest.RequestTime = currentTime.Add(service.ClientCallDelay);
                    clientRequest.IsPriority = isPriority;
                    clientRequest.Type = ClientRequestType.Live;

                    var error = clientRequest.Validate().FirstOrDefault();
                    if (error != null)
                    {
                        throw new FaultException(error.Message);
                    }

                    session.Save(clientRequest);

                    var queueEvent = new ClientRequestEvent()
                    {
                        ClientRequest = clientRequest,
                        Message = string.Format("[{0}] добавил новый запрос клиента в живую очередь [{1}]", currentUser, clientRequest)
                    };

                    session.Save(queueEvent);

                    var defaultConfig = session.Get<DefaultConfig>(ConfigType.Default);
                    if (defaultConfig.IsDebug)
                    {
                        session.Save(new QueuePlanReport()
                        {
                            ClientRequest = clientRequest,
                            Report = string.Join(Environment.NewLine, todayQueuePlan.Report)
                        });
                    }

                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.AddClientRequest(clientRequest);
                        todayQueuePlan.Build(currentTime);
                    }

                    queueInstance.Event(queueEvent);

                    logger.Info("Запрос добавлен в план очереди");

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }

        public async Task<DTO.ClientRequestCoupon> GetClientRequestCoupon(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    ClientRequest clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId),
                            string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    DefaultConfig defaultConfig = session.Get<DefaultConfig>(ConfigType.Default);

                    bool isToday = clientRequest.RequestDate == DateTime.Today;

                    var parameters = session.CreateCriteria<ClientRequestParameter>()
                        .Add(Restrictions.Eq("ClientRequest", clientRequest))
                        .List<ClientRequestParameter>();

                    DTO.ClientRequestCoupon clientRequestCoupon = new DTO.ClientRequestCoupon()
                    {
                        QueueName = defaultConfig.QueueName,
                        IsToday = isToday,
                        Number = clientRequest.Number,
                        CreateDate = clientRequest.CreateDate,
                        RequestDate = clientRequest.RequestDate,
                        RequestTime = clientRequest.RequestTime,
                        Subjects = clientRequest.Subjects,
                        Parameters = Mapper.Map<IEnumerable<ClientRequestParameter>, DTO.ClientRequestParameter[]>(parameters),
                        Service = Mapper.Map<Service, DTO.Service>(clientRequest.Service),
                        Workplaces = Mapper.Map<Workplace[], DTO.Workplace[]>(GetCouponWorkplaces(clientRequest))
                    };

                    if (clientRequest.Client != null)
                    {
                        clientRequestCoupon.Client = Mapper.Map<Client, DTO.Client>(clientRequest.Client);
                    }

                    if (clientRequest.Type == ClientRequestType.Live && isToday)
                    {
                        var todayQueuePlan = queueInstance.TodayQueuePlan;
                        using (var locker = todayQueuePlan.ReadLock())
                        {
                            ClientRequestPlan clientRequestPlan = todayQueuePlan.OperatorsPlans
                                             .SelectMany(p => p.ClientRequestPlans)
                                             .SingleOrDefault(p => p.ClientRequest.Equals(clientRequest));
                            if (clientRequestPlan != null)
                            {
                                clientRequestCoupon.HasPlanned = true;
                                clientRequestCoupon.Position = clientRequestPlan.Position;
                                clientRequestCoupon.WaitingTime = clientRequestPlan.StartTime - DateTime.Now.TimeOfDay;
                            }
                        }
                    }

                    return clientRequestCoupon;
                }
            });
        }

        public async Task DeleteClientRequest(Guid clientRequestId)
        {
            await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId),
                            string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    session.Delete(clientRequest);
                    transaction.Commit();
                }
            });
        }

        private Workplace[] GetCouponWorkplaces(ClientRequest clientRequest)
        {
            QueuePlan queuePlan;

            if (clientRequest.RequestDate == DateTime.Today)
            {
                queuePlan = queueInstance.TodayQueuePlan;
            }
            else
            {
                queuePlan = new QueuePlan();
                queuePlan.Load(clientRequest.RequestDate);
                queuePlan.Build();
            }

            using (var locker = queuePlan.ReadLock())
            {
                ServiceRenderingMode serviceRenderingMode = clientRequest.Type == ClientRequestType.Early
                                                            ? ServiceRenderingMode.EarlyRequests
                                                            : ServiceRenderingMode.LiveRequests;

                Schedule serviceSchedule = queuePlan.GetServiceSchedule(clientRequest.Service);

                return queuePlan.GetServiceRenderings(serviceSchedule, clientRequest.ServiceStep, serviceRenderingMode)
                      .Where(r => r.Operator.Workplace != null)
                      .Select(r => r.Operator.Workplace)
                      .ToArray();
            }
        }

        public async Task<DTO.ClientRequest> EditClientRequest(DTO.ClientRequest source)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.ClientsRequests);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Guid clientRequestId = source.Id;

                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId),
                            string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    if (!clientRequest.IsEditable)
                    {
                        throw new FaultException(string.Format("Невозможно изменить тип запрос клиента [{0}]", clientRequest));
                    }

                    if (!clientRequest.Type.Equals(source.Type))
                    {
                        clientRequest.Type = source.Type;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] изменил тип на [{1}] для запроса клиента [{2}]", currentUser,
                                Translater.Enum(clientRequest.Type), clientRequest)
                        });
                    }

                    if (clientRequest.IsPriority != source.IsPriority)
                    {
                        clientRequest.IsPriority = source.IsPriority;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] {1} для запроса клиента [{2}]", currentUser,
                                clientRequest.IsPriority ? "установил приоритет" : "снял приоритет", clientRequest)
                        });
                    }

                    if (clientRequest.Subjects != source.Subjects)
                    {
                        clientRequest.Subjects = source.Subjects;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] изменил количество объектов на [{1}] для [{2}]",
                                currentUser, clientRequest.Subjects, clientRequest)
                        });
                    }

                    if (clientRequest.RequestDate != source.RequestDate)
                    {
                        clientRequest.RequestDate = source.RequestDate;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] изменил дату запроса с на [{1}]",
                                currentUser, clientRequest.RequestDate)
                        });
                    }

                    if (clientRequest.RequestTime != source.RequestTime)
                    {
                        clientRequest.RequestTime = source.RequestTime;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] изменил время запроса с на [{1:hh\\:mm}]",
                                currentUser, clientRequest.RequestTime)
                        });
                    }

                    if (source.Service != null)
                    {
                        Guid serviceId = source.Service.Id;

                        Service service = session.Get<Service>(serviceId);
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
                                Message = string.Format("[{0}] изменил услугу на [{1}] для запроса клиента [{2}]", currentUser, service, clientRequest)
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
                                Message = string.Format("[{0}] изменил этап услуги на [{1}] для запроса клиента [{2}]", currentUser, serviceStep, clientRequest)
                            });
                        }
                    }
                    else if (clientRequest.ServiceStep != null)
                    {
                        clientRequest.ServiceStep = null;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] сбросил этап услуги для запроса клиента [{1}]", currentUser, clientRequest)
                        });
                    }

                    if (source.Operator != null)
                    {
                        Guid operatorId = source.Operator.Id;

                        var queueOperator = session.Get<Operator>(operatorId);
                        if (queueOperator == null)
                        {
                            throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(operatorId),
                                string.Format("Оператор [{0}] не найден", operatorId));
                        }

                        if (clientRequest.Operator == null
                            || !clientRequest.Operator.Equals(queueOperator))
                        {
                            clientRequest.Operator = queueOperator;

                            session.Save(new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("[{0}] установил оператора [{1}] для запроса клиента [{2}]", currentUser, queueOperator, clientRequest)
                            });
                        }
                    }
                    else if (clientRequest.Operator != null)
                    {
                        clientRequest.Operator = null;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] сбросил оператора для запроса клиента [{1}]", currentUser, clientRequest)
                        });
                    }

                    var error = clientRequest.Validate().FirstOrDefault();
                    if (error != null)
                    {
                        throw new FaultException(error.Message);
                    }

                    clientRequest.Version++;
                    session.Save(clientRequest);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.AddClientRequest(clientRequest);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }

        public async Task<DTO.ClientRequest> CancelClientRequest(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.ClientsRequests);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId), string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    try
                    {
                        clientRequest.Cancel();
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }

                    clientRequest.Version++;
                    session.Save(clientRequest);

                    var queueEvent = new ClientRequestEvent()
                    {
                        ClientRequest = clientRequest,
                        Message = string.Format("[{0}] отмененил запрос клиента [{1}]", currentUser, clientRequest)
                    };

                    session.Save(queueEvent);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(clientRequest);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    queueInstance.Event(queueEvent);

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }

        public async Task<DTO.ClientRequest> RestoreClientRequest(Guid clientRequestId)
        {
            return await Task.Run(() =>
            {
                CheckPermission(UserRole.Administrator, AdministratorPermissions.ClientsRequests);

                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId),
                            string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    try
                    {
                        clientRequest.Restore();
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }

                    clientRequest.Version++;
                    session.Save(clientRequest);

                    session.Save(new ClientRequestEvent()
                    {
                        ClientRequest = clientRequest,
                        Message = string.Format("[{0}] восстановил запрос клиента [{0}]", currentUser, clientRequest)
                    });

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.AddClientRequest(clientRequest);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }

        public async Task<DTO.ClientRequest> PostponeClientRequest(Guid clientRequestId, TimeSpan postponeTime)
        {
            return await Task.Run(() =>
            {
                using (var session = sessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientRequest = session.Get<ClientRequest>(clientRequestId);
                    if (clientRequest == null)
                    {
                        throw new FaultException<ObjectNotFoundFault>(new ObjectNotFoundFault(clientRequestId), string.Format("Запрос клиента [{0}] не найден", clientRequestId));
                    }

                    try
                    {
                        clientRequest.Postpone(postponeTime);
                    }
                    catch (Exception exception)
                    {
                        throw new FaultException(exception.Message);
                    }

                    clientRequest.Version++;
                    session.Save(clientRequest);

                    var queueEvent = new ClientRequestEvent()
                    {
                        ClientRequest = clientRequest,
                        Message = string.Format("[{0}] отложил на [{1} мин.] запрос клиента [{2}]", currentUser, postponeTime.TotalMinutes, clientRequest)
                    };

                    session.Save(queueEvent);

                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                    using (var locker = todayQueuePlan.WriteLock())
                    {
                        transaction.Commit();

                        todayQueuePlan.Put(clientRequest);
                        todayQueuePlan.Build(DateTime.Now.TimeOfDay);
                    }

                    queueInstance.Event(queueEvent);

                    return Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest);
                }
            });
        }
    }
}