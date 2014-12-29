using AutoMapper;
using NHibernate.Criterion;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;
using ValidationError = Junte.Data.NHibernate.ValidationError;

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

                    var firstServiceStep = session.CreateCriteria<ServiceStep>()
                        .Add(Restrictions.Eq("Service", service))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1)
                        .UniqueResult<ServiceStep>();

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
                            var freeTime = queuePlan.GetServiceFreeTime(service, firstServiceStep, ClientRequestType.Early, subjects);
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

                    var clientRequest = new ClientRequest()
                    {
                        Client = client,
                        Service = service,
                        ServiceStep = firstServiceStep,
                        Subjects = subjects
                    };

                    /*foreach (var p in service.Parameters)
                    {
                        if (!parameters.ContainsKey(p.Id))
                        {
                            throw new FaultException(string.Format("Не удалось найти параметр услуги [{0}]", p));
                        }

                        try
                        {
                            clientRequest.Parameters.Add(p.Compile(parameters[p.Id]));
                        }
                        catch (Exception exception)
                        {
                            throw new FaultException(exception.Message);
                        }
                    }*/

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

                    var firstServiceStep = session.CreateCriteria<ServiceStep>()
                        .Add(Restrictions.Eq("Service", service))
                        .AddOrder(Order.Asc("SortId"))
                        .SetMaxResults(1)
                        .UniqueResult<ServiceStep>();

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
                            var freeTime = todayQueuePlan.GetServiceFreeTime(service, firstServiceStep, ClientRequestType.Live, service.IsPlanSubjects ? subjects : 1);
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

                    var clientRequest = new ClientRequest()
                    {
                        Client = client,
                        Service = service,
                        ServiceStep = firstServiceStep,
                        Subjects = subjects,
                        IsPriority = client != null && client.VIP
                    };

                    /*foreach (var p in service.Parameters)
                    {
                        if (!parameters.ContainsKey(p.Id))
                        {
                            throw new FaultException(string.Format("Не удалось найти параметр услуги [{0}]", p));
                        }

                        try
                        {
                            clientRequest.Parameters.Add(p.Compile(parameters[p.Id]));
                        }
                        catch (Exception exception)
                        {
                            throw new FaultException(exception.Message);
                        }
                    }*/

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

        public async Task<string> GetClientRequestCoupon(Guid clientRequestId)
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

                    string output = string.Empty;

                    var thread = new Thread(new ThreadStart(() =>
                    {
                        var couponConfig = session.Get<CouponConfig>(ConfigType.Coupon);

                        var xmlReader = new XmlTextReader(new StringReader(couponConfig.Template));
                        var grid = XamlReader.Load(xmlReader) as Grid;

                        var defaultConfig = session.Get<DefaultConfig>(ConfigType.Default);

                        object element = grid.FindName("queueName");
                        if (element != null)
                        {
                            try
                            {
                                ((TextBlock)element).Text = defaultConfig.QueueName;
                            }
                            catch { }
                        }

                        element = grid.FindName("clientRequestBlock");
                        if (element != null)
                        {
                            var clientRequestBlock = (FrameworkElement)element;
                            clientRequestBlock.Visibility = Visibility.Visible;

                            element = clientRequestBlock.FindName("number");
                            if (element != null)
                            {
                                try
                                {
                                    ((TextBlock)element).Text = clientRequest.Number.ToString();
                                }
                                catch { }
                            }
                            element = clientRequestBlock.FindName("createDate");
                            if (element != null)
                            {
                                try
                                {
                                    ((TextBlock)element).Text = clientRequest.CreateDate.ToString();
                                }
                                catch { }
                            }
                            element = clientRequestBlock.FindName("requestDate");
                            if (element != null)
                            {
                                try
                                {
                                    ((TextBlock)element).Text = clientRequest.RequestDate.ToShortDateString();
                                }
                                catch { }
                            }
                            element = clientRequestBlock.FindName("requestTime");
                            if (element != null)
                            {
                                try
                                {
                                    ((TextBlock)element).Text = clientRequest.RequestTime.ToString("hh\\:mm\\:ss");
                                }
                                catch { }
                            }
                            element = clientRequestBlock.FindName("subjects");
                            if (element != null)
                            {
                                try
                                {
                                    ((TextBlock)element).Text = clientRequest.Subjects.ToString();
                                }
                                catch { }
                            }
                        }

                        var client = clientRequest.Client;

                        element = grid.FindName("clientBlock");
                        if (client != null && element != null)
                        {
                            var clientBlock = (FrameworkElement)element;
                            clientBlock.Visibility = Visibility.Visible;

                            element = clientBlock.FindName("client");
                            if (element != null)
                            {
                                ((TextBlock)element).Text = client.ToString();
                            }
                        }

                        element = grid.FindName("parametersGrid");
                        if (element != null)
                        {
                            var parametersGrid = ((Grid)element);
                            parametersGrid.Visibility = Visibility.Visible;

                            int row = 0;
                            foreach (var p in clientRequest.Parameters)
                            {
                                parametersGrid.RowDefinitions.Add(new RowDefinition());

                                var label = new Label()
                                {
                                    Content = p.Name
                                };
                                label.SetValue(Grid.ColumnProperty, 0);
                                label.SetValue(Grid.RowProperty, row);
                                parametersGrid.Children.Add(label);

                                var textBlock = new TextBlock()
                                {
                                    Text = p.Value
                                };
                                textBlock.SetValue(Grid.ColumnProperty, 1);
                                textBlock.SetValue(Grid.RowProperty, row);
                                parametersGrid.Children.Add(textBlock);

                                row++;
                            }
                        }

                        var service = clientRequest.Service;

                        element = grid.FindName("serviceBlock");
                        if (element != null)
                        {
                            var serviceBlock = (FrameworkElement)element;
                            serviceBlock.Visibility = Visibility.Visible;

                            element = serviceBlock.FindName("serviceName");
                            if (element != null)
                            {
                                try
                                {
                                    ((TextBlock)element).Text = service.ToString();
                                }
                                catch { }
                            }
                        }

                        var requestDate = clientRequest.RequestDate;
                        var isRequestDateToday = requestDate == DateTime.Today;

                        element = grid.FindName("workplacesBlock");
                        if (element != null)
                        {
                            var workplacesBlock = (FrameworkElement)element;
                            workplacesBlock.Visibility = Visibility.Visible;

                            element = workplacesBlock.FindName("workplaces");
                            if (element != null)
                            {
                                QueuePlan queuePlan;

                                if (isRequestDateToday)
                                {
                                    queuePlan = queueInstance.TodayQueuePlan;
                                }
                                else
                                {
                                    queuePlan = new QueuePlan();
                                    queuePlan.Load(requestDate);
                                    queuePlan.Build();
                                }

                                var workplaces = new List<string>();

                                using (var locker = queuePlan.ReadLock())
                                {
                                    var serviceRenderingMode = clientRequest.Type == ClientRequestType.Early
                                        ? ServiceRenderingMode.EarlyRequests
                                        : ServiceRenderingMode.LiveRequests;

                                    var schedule = queuePlan.GetServiceSchedule(clientRequest.Service);
                                    foreach (var r in queuePlan.GetServiceRenderings(schedule, clientRequest.ServiceStep, serviceRenderingMode))
                                    {
                                        var workplace = r.Operator.Workplace;
                                        if (workplace != null)
                                        {
                                            workplaces.Add(workplace.ToString());
                                        }
                                    }
                                    if (workplaces.Count > 0)
                                    {
                                        try
                                        {
                                            ((TextBlock)element).Text = string.Join(", ", workplaces);
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }

                        element = grid.FindName("stateBlock");
                        if (element != null)
                        {
                            var stateBlock = (StackPanel)element;

                            if (clientRequest.Type == ClientRequestType.Live && isRequestDateToday)
                            {
                                stateBlock.Visibility = Visibility.Visible;

                                try
                                {
                                    var todayQueuePlan = queueInstance.TodayQueuePlan;
                                    using (var locker = todayQueuePlan.ReadLock())
                                    {
                                        foreach (var o in todayQueuePlan.OperatorsPlans)
                                        {
                                            var clientRequestPlan = o.ClientRequestPlans
                                                .FirstOrDefault(p => p.ClientRequest.Equals(clientRequest));
                                            if (clientRequestPlan != null)
                                            {
                                                element = grid.FindName("position");
                                                if (element != null)
                                                {
                                                    ((TextBlock)element).Text = clientRequestPlan.Position.ToString();
                                                }

                                                var waitingTime = clientRequestPlan.StartTime - DateTime.Now.TimeOfDay;
                                                element = grid.FindName("waitingTime");
                                                if (element != null)
                                                {
                                                    try
                                                    {
                                                        ((TextBlock)element).Text = Math.Round(waitingTime.TotalMinutes).ToString();
                                                    }
                                                    catch { }
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception exception)
                                {
                                    logger.Warn(exception);
                                }
                            }
                        }

                        output = XamlWriter.Save(grid);
                    }));

                    thread.SetApartmentState(ApartmentState.STA);
                    thread.IsBackground = true;
                    thread.Start();
                    thread.Join();

                    return output;
                }
            });
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
                                clientRequest.Type.Translate(), clientRequest)
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

                            session.Save(new ClientRequestEvent()
                            {
                                ClientRequest = clientRequest,
                                Message = string.Format("[{0}] изменил услугу на [{1}] для запроса клиента [{2}]", currentUser, service, clientRequest)
                            });
                        }
                    }
                    else
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
                    else
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
                    else
                    {
                        clientRequest.Operator = null;

                        session.Save(new ClientRequestEvent()
                        {
                            ClientRequest = clientRequest,
                            Message = string.Format("[{0}] сбросил оператора для запроса клиента [{1}]", currentUser, clientRequest)
                        });
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