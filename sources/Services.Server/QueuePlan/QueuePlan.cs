using Junte.Data.NHibernate;
using Junte.Parallel;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Criterion;
using NLog;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using ServiceRenderingKey = System.Tuple<Queue.Model.Schedule,
                            Queue.Model.ServiceStep,
                            Queue.Model.Common.ServiceRenderingMode>;

namespace Queue.Services.Server
{
    [Flags]
    public enum QueuePlanFlushMode
    {
        ServiceSchedule = 1,
        ServiceRenderings = 2,
        OperatorInterruptions = 4,
        Full = ServiceSchedule | ServiceRenderings | OperatorInterruptions
    }

    public sealed class QueuePlan : Synchronized, IDisposable
    {
        #region dependency

        [Dependency]
        public SessionProvider SessionProvider { get; set; }

        #endregion dependency

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private List<ClientRequest> clientRequests = new List<ClientRequest>();

        private readonly Dictionary<Operator, TimeInterval[]> operatorInterruptions
            = new Dictionary<Operator, TimeInterval[]>();

        private readonly Dictionary<ServiceRenderingKey, ServiceRendering[]> serviceRenderings
            = new Dictionary<ServiceRenderingKey, ServiceRendering[]>();

        private readonly Dictionary<Service, Schedule> serviceSchedule
            = new Dictionary<Service, Schedule>();

        private readonly EntityStorage storage = new EntityStorage();

        public QueuePlan()
        {
            logger.Info("Создание экземпляра плана очереди");

            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this);

            OperatorsPlans = new List<OperatorPlan>();
            NotDistributedClientRequests = new List<NotDistributedClientRequest>();
        }

        public event EventHandler<QueuePlanEventArgs> OnBuilded;

        public event EventHandler<QueuePlanEventArgs> OnCurrentClientRequestPlanUpdated;

        public event EventHandler<QueuePlanEventArgs> OnOperatorPlanMetricsUpdated;

        /// <summary>
        /// Последний номер талона
        /// </summary>
        public int LastNumber { get; set; }

        public List<NotDistributedClientRequest> NotDistributedClientRequests { get; private set; }

        public List<OperatorPlan> OperatorsPlans { get; private set; }

        public DateTime PlanDate { get; private set; }

        public TimeSpan PlanTime { get; private set; }

        public List<string> Report { get; private set; }

        public int Version { get; private set; }

        /// <summary>
        /// Запланировать новый запрос клиента
        /// </summary>
        /// <param name="clientRequest"></param>
        /// <returns></returns>
        public void AddClientRequest(ClientRequest clientRequest)
        {
            clientRequests.Add(storage.Put(clientRequest));
        }

        public void Build()
        {
            Build(TimeSpan.Zero);
        }

        /// <summary>
        /// Построить план очереди
        /// </summary>
        /// <returns></returns>
        public void Build(TimeSpan planTime)
        {
            PlanTime = planTime;

            Report = new List<string>()
            {
                string.Format("Построение плана очереди на дату {0:dd.MM.yyyy} и время {1:hh\\:mm\\:ss}", PlanDate, PlanTime)
            };

            foreach (var o in OperatorsPlans)
            {
                clientRequests.AddRange(o.ClientRequestPlans.Select(p => p.ClientRequest));
                o.ClientRequestPlans.Clear();

                o.PlanTime = PlanTime;
            }

            clientRequests.AddRange(NotDistributedClientRequests.Select(n => n.ClientRequest));
            NotDistributedClientRequests.Clear();

            var conditions = new[] {
                new {
                    Name = "Вызываемые",
                    Predicate = new Predicate<ClientRequest>(r => r.State == ClientRequestState.Calling)
                },
                new {
                    Name = "Обслуживаемые",
                    Predicate = new Predicate<ClientRequest>(r => r.State == ClientRequestState.Rendering)
                },
                new {
                    Name = "Отложенные",
                    Predicate = new Predicate<ClientRequest>(r => r.State == ClientRequestState.Postponed)
                },
                new {
                    Name = "Предварительная запись с определенными операторами",
                    Predicate = new Predicate<ClientRequest>(r => r.Type == ClientRequestType.Early && r.Operator != null)
                },
                new {
                    Name = "Предварительная запись",
                    Predicate = new Predicate<ClientRequest>(r => r.Type == ClientRequestType.Early)
                },
                new {
                    Name = "Живая очередь с приоритетом",
                    Predicate = new Predicate<ClientRequest>(r => r.Type == ClientRequestType.Live && r.IsPriority)
                },
                new {
                    Name = "Живая очередь",
                    Predicate = new Predicate<ClientRequest>(r => r.Type == ClientRequestType.Live)
                }
            };

            clientRequests = clientRequests.Distinct().ToList();

            foreach (var condition in conditions)
            {
                var conditionClientRequests = clientRequests
                    .Where(new Func<ClientRequest, bool>(condition.Predicate))
                    .OrderByDescending(p => p.Service.Priority)
                    .ThenBy(p => p.RequestTime)
                    .ThenBy(p => p.CreateDate);

                Report.Add(string.Format("Планируется по условию [{0}] [{1}] запросов", condition.Name, conditionClientRequests.Count()));

                foreach (var conditionClientRequest in conditionClientRequests)
                {
                    Report.Add(string.Format("Планируется запрос [{0}]", conditionClientRequest));

                    if (conditionClientRequest.RequestDate != PlanDate)
                    {
                        Report.Add("Дата запроса не соответствует дате плана очереди");
                        continue;
                    }

                    var schedule = GetServiceSchedule(conditionClientRequest.Service);

                    Report.Add(string.Format("Определено расписание для услуги [{0}]", schedule));

                    try
                    {
                        OperatorPlan targetOperatorPlan;

                        if (conditionClientRequest.Operator != null)
                        {
                            Report.Add(string.Format("Оператор {0} определен ранее для запроса", conditionClientRequest.Operator));

                            targetOperatorPlan = GetOperatorPlan(conditionClientRequest.Operator);
                        }
                        else
                        {
                            var serviceRenderingMode = conditionClientRequest.Type == ClientRequestType.Early
                                ? ServiceRenderingMode.EarlyRequests
                                : ServiceRenderingMode.LiveRequests;

                            var renderings = GetServiceRenderings(schedule, conditionClientRequest.ServiceStep, serviceRenderingMode);

                            Report.Add("Поиск потенциальных операторов");

                            var potentialOperatorsPlans = renderings
                                .Where(r => r.Operator.IsActive)
                                .Select(r => new
                                {
                                    OperatorPlan = OperatorsPlans.FirstOrDefault(o => o.Operator.Equals(r.Operator)),
                                    Priority = r.Priority
                                }).Where(r => r.OperatorPlan != null);

                            if (potentialOperatorsPlans.Count() == 0)
                            {
                                Report.Add("Операторы не найдены");

                                throw new Exception("В системе нет операторов способных обработать запрос");
                            }

                            var requestTime = conditionClientRequest.RequestTime;

                            if (requestTime < planTime)
                            {
                                requestTime = planTime;
                            }

                            Report.Add(string.Format("Определено расписание для услуги [{0}]", schedule));

                            var operatorPlanMetrics = potentialOperatorsPlans
                                .Select(p => new
                                {
                                    OperatorPlan = p.OperatorPlan,
                                    NearTimeInterval = p.OperatorPlan.GetNearTimeInterval(requestTime, schedule, GetOperatorInterruptions(p.OperatorPlan.Operator), conditionClientRequest.Subjects),
                                    IsOnline = p.OperatorPlan.Operator.Online,
                                    Availability = p.OperatorPlan.CurrentClientRequestPlan == null || !p.OperatorPlan.CurrentClientRequestPlan.ClientRequest.InWorking,
                                    Priority = p.Priority,
                                    Workload = p.OperatorPlan.Metrics.Workload
                                })
                                .OrderBy(m => m.NearTimeInterval)
                                .ThenByDescending(m => m.IsOnline)
                                .ThenByDescending(m => m.Availability)
                                .ThenByDescending(m => m.Priority)
                                .ThenBy(m => m.Workload)
                                .ToList();

                            Report.Add("Расчет метрик операторов");

                            foreach (var m in operatorPlanMetrics)
                            {
                                m.OperatorPlan.Metrics.Standing++;

                                Report.Add(string.Format("{0} {1}, ближайший интервал времени: {2:hh\\:mm\\:ss}, доступность: {3}, приоритет: {4}, нагрузка: {5:hh\\:mm\\:ss}",
                                    m.OperatorPlan, m.IsOnline ? "в сети" : "не в сети", m.NearTimeInterval, m.Availability ? "свободен" : "занят", m.Priority, m.Workload));
                            }

                            targetOperatorPlan = operatorPlanMetrics.Select(m => m.OperatorPlan).First();
                        }

                        if (targetOperatorPlan != null)
                        {
                            targetOperatorPlan.AddClientRequest(conditionClientRequest, schedule, GetOperatorInterruptions(targetOperatorPlan.Operator));

                            Report.Add(string.Format("Оператор [{0}] назначен для [{1}]", targetOperatorPlan, conditionClientRequest));
                        }
                        else
                        {
                            throw new Exception("Не удалось определить целевой план оператора");
                        }
                    }
                    catch (Exception exception)
                    {
                        NotDistributedClientRequests.Add(new NotDistributedClientRequest(conditionClientRequest, exception.Message));
                    }

                    Report.Add(string.Empty);
                }

                clientRequests.RemoveAll(condition.Predicate);
            }

            foreach (var r in clientRequests)
            {
                NotDistributedClientRequests.Add(new NotDistributedClientRequest(r, "Ни один оператор не может обслужить запрос клиента"));
            }

            clientRequests.Clear();

            Report.Add(string.Empty);

            foreach (var o in OperatorsPlans)
            {
                if (o.CurrentClientRequestPlan != null)
                {
                    Report.Add(string.Format("{0} текущий запрос [{1}]", o.Operator, o.CurrentClientRequestPlan));
                }

                foreach (var p in o.ClientRequestPlans)
                {
                    Report.Add(string.Format("{0} запланирован в {1:hh\\:mm\\:ss} {2}", p.Position, p.StartTime, p));
                }
            }

            if (OnCurrentClientRequestPlanUpdated != null)
            {
                logger.Info("Запуск обработчика для события [OnCurrentClientRequestPlanUpdated] с кол-вом слушателей [{0}]",
                    OnCurrentClientRequestPlanUpdated.GetInvocationList().Length);

                foreach (var o in OperatorsPlans)
                {
                    var e = new QueuePlanEventArgs()
                    {
                        Operator = o.Operator
                    };

                    if (o.CurrentClientRequestPlan != null)
                    {
                        e.ClientRequestPlan = o.CurrentClientRequestPlan;
                    }

                    OnCurrentClientRequestPlanUpdated(this, e);
                }
            }

            if (OnOperatorPlanMetricsUpdated != null)
            {
                logger.Info("Запуск обработчика для события [OnOperatorPlanMetricsUpdated] с кол-вом слушателей [{0}]",
                    OnOperatorPlanMetricsUpdated.GetInvocationList().Length);

                foreach (var o in OperatorsPlans)
                {
                    OnOperatorPlanMetricsUpdated(this, new QueuePlanEventArgs()
                    {
                        OperatorPlanMetrics = o.Metrics
                    });
                }
            }

            Version++;

            logger.Info("Построение плана очереди завершено с версией [{0}]", Version);

            if (OnBuilded != null)
            {
                OnBuilded(this, new QueuePlanEventArgs());
            }
        }

        public void Dispose()
        {
            logger.Info("Уничтожение экземпляра плана очереди");
        }

        public void Flush(QueuePlanFlushMode mode)
        {
            if (mode.HasFlag(QueuePlanFlushMode.ServiceSchedule))
            {
                serviceSchedule.Clear();
            }

            if (mode.HasFlag(QueuePlanFlushMode.ServiceRenderings))
            {
                serviceRenderings.Clear();
            }

            if (mode.HasFlag(QueuePlanFlushMode.OperatorInterruptions))
            {
                operatorInterruptions.Clear();
            }
        }

        /// <summary>
        /// Взять текущий план обслуживания оператора
        /// </summary>
        /// <param name="queueOperator"></param>
        /// <returns></returns>
        public OperatorPlan GetOperatorPlan(Operator queueOperator)
        {
            OperatorPlan operatorPlan = OperatorsPlans
                .FirstOrDefault(p => p.Operator.Equals(queueOperator));
            if (operatorPlan == null)
            {
                throw new Exception(string.Format("План обслуживания для оператора [{0}] не найден", queueOperator));
            }

            return operatorPlan;
        }

        /// <summary>
        /// Получить доступные временные интервалы
        /// </summary>
        public ServiceFreeTime GetServiceFreeTime(Service service, ServiceStep serviceStep, ClientRequestType requestType, int subjects = 1)
        {
            Schedule schedule = GetServiceSchedule(service);
            if (schedule == null)
            {
                throw new Exception("Не удалось определить расписание для услуги");
            }

            if (!schedule.IsWorked)
            {
                throw new Exception("На указанную дату услуга не оказывается");
            }

            switch (requestType)
            {
                case ClientRequestType.Live:
                    if (!schedule.RenderingMode.HasFlag(ServiceRenderingMode.LiveRequests))
                    {
                        throw new Exception("Живая очередь для данной услуги на указанный день отключена");
                    }
                    break;

                case ClientRequestType.Early:
                    if (!schedule.RenderingMode.HasFlag(ServiceRenderingMode.EarlyRequests))
                    {
                        throw new Exception("Предварительная запись для данной услуги на указанный день отключена");
                    }

                    break;
            }

            // Только подходящие параметры обслуживания
            var serviceRenderingMode = requestType == ClientRequestType.Early
                ? ServiceRenderingMode.EarlyRequests
                : ServiceRenderingMode.LiveRequests;

            var renderings = GetServiceRenderings(schedule, serviceStep, serviceRenderingMode);

            var potentialOperatorsPlans = OperatorsPlans
                .Where(p => renderings.Any(r => r.Operator.IsActive && r.Operator.Equals(p.Operator)))
                .ToList();

            if (potentialOperatorsPlans.Count == 0)
            {
                throw new Exception("В системе нет операторов способных оказать услугу");
            }

            var report = new List<string>()
            {
                string.Format("Поиск свободного времени на дату {0:dd.MM.yyyy} и время {1:hh\\:mm\\:ss}", PlanDate, PlanTime)
            };

            var timeIntervals = new List<TimeSpan>();

            var startTime = schedule.StartTime;
            if (PlanTime > startTime)
            {
                startTime = PlanTime;
            }

            report.Add(string.Format("Поиск интервалов времени с {0:hh\\:mm\\:ss}", startTime));

            var clientInterval = requestType == ClientRequestType.Live
                ? schedule.LiveClientInterval : schedule.EarlyClientInterval;
            clientInterval = TimeSpan.FromTicks(clientInterval.Ticks * subjects);

            int openedRequests = 0;

            foreach (var potentialOperatorPlan in potentialOperatorsPlans)
            {
                openedRequests += potentialOperatorPlan.ClientRequestPlans
                    .Select(p => p.ClientRequest)
                    .Count(r => r.Type == requestType);

                report.Add(string.Format("В плане оператора [{0}] {1} открытых запросов", potentialOperatorPlan, openedRequests));

                var intervalTime = startTime;

                while (intervalTime < schedule.FinishTime)
                {
                    intervalTime = potentialOperatorPlan.GetNearTimeInterval(intervalTime, schedule, GetOperatorInterruptions(potentialOperatorPlan.Operator), subjects);

                    report.Add(string.Format("Найден ближайший интвервал времени {0:hh\\:mm\\:ss}", intervalTime));

                    var timeIntervalRounding = service.TimeIntervalRounding;
                    if (timeIntervalRounding != TimeSpan.Zero)
                    {
                        var rounding = new TimeSpan(intervalTime.Ticks % timeIntervalRounding.Ticks);
                        if (rounding != TimeSpan.Zero)
                        {
                            report.Add(string.Format("Округление интервала {0:hh\\:mm\\:ss}", rounding));

                            intervalTime = intervalTime.Add(rounding <= schedule.Intersection ? -rounding : timeIntervalRounding.Subtract(rounding));
                        }
                    }

                    report.Add(string.Format("Найден интвервал времени {0:hh\\:mm\\:ss}", intervalTime));

                    timeIntervals.Add(intervalTime);
                    intervalTime = intervalTime.Add(clientInterval);

                    if (timeIntervals.Count() > 1000)
                    {
                        throw new OverflowException("Возможно переполнение памяти. Обратитесь к администратору.");
                    }
                }
            }

            int freeTimeIntervals;

            if (PlanDate != DateTime.Today)
            {
                int maxRequests = (int)((schedule.EarlyFinishTime - schedule.EarlyStartTime).Ticks / clientInterval.Ticks) * potentialOperatorsPlans.Count; ;
                report.Add(string.Format("Максимальное кол-во запросов {0}", maxRequests));

                int maxEarlyClientRequests = maxRequests * schedule.EarlyReservation / 100;
                report.Add(string.Format("Максимальное кол-во запросов с учетом резервирования {0}", maxEarlyClientRequests));

                freeTimeIntervals = maxEarlyClientRequests - openedRequests;
                if (freeTimeIntervals <= 0)
                {
                    throw new Exception("Возможности резервирования для данной услуги на указанный день исчерпаны");
                }

                timeIntervals.RemoveAll(i => i < schedule.EarlyStartTime);
                timeIntervals.RemoveAll(i => i.Add(clientInterval - schedule.Intersection) > schedule.EarlyFinishTime);

                if (freeTimeIntervals > timeIntervals.Count)
                {
                    freeTimeIntervals = timeIntervals.Count;
                }
            }
            else
            {
                timeIntervals.RemoveAll(i => i.Add(clientInterval - schedule.Intersection) > schedule.FinishTime);

                freeTimeIntervals = timeIntervals.Count;
            }

            return new ServiceFreeTime()
            {
                Service = service,
                Schedule = schedule,
                TimeIntervals = timeIntervals.ToArray(),
                FreeTimeIntervals = freeTimeIntervals,
                Report = report
            };
        }

        /// <summary>
        /// Получить параметры обслуживания
        /// </summary>
        public ServiceRendering[] GetServiceRenderings(Schedule schedule, ServiceStep serviceStep, ServiceRenderingMode serviceRenderingMode)
        {
            ServiceRenderingKey key = new ServiceRenderingKey(schedule, serviceStep, serviceRenderingMode);

            if (!serviceRenderings.ContainsKey(key))
            {
                logger.Info("Загрузка параметров оказания услуги для раписания [{0}]", schedule);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    serviceRenderings.Add(key, session.CreateCriteria<ServiceRendering>()
                         .Add(Restrictions.Eq("Schedule", schedule))
                         .Add(serviceStep != null ? Restrictions.Eq("ServiceStep", serviceStep) : Restrictions.IsNull("ServiceStep"))
                         .Add(new Disjunction()
                             .Add(Restrictions.Eq("Mode", serviceRenderingMode))
                             .Add(Restrictions.Eq("Mode", ServiceRenderingMode.AllRequests)))
                         .List<ServiceRendering>()
                         .ToArray());

                    transaction.Commit();
                }
            }

            return serviceRenderings[key];
        }

        /// <summary>
        /// Получить перерывы оператора
        /// </summary>
        public TimeInterval[] GetOperatorInterruptions(Operator queueOperator)
        {
            if (!operatorInterruptions.ContainsKey(queueOperator))
            {
                logger.Info("Загрузка перерывов оператора [{0}]", queueOperator);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    operatorInterruptions.Add(queueOperator, session.CreateCriteria<OperatorInterruption>()
                         .Add(Restrictions.Eq("Operator", queueOperator))
                         .Add(new Disjunction()
                            .Add(new Conjunction()
                                .Add(Restrictions.Eq("Type", OperatorInterruptionType.Weekday))
                                .Add(Restrictions.Eq("DayOfWeek", PlanDate.DayOfWeek)))
                            .Add(new Conjunction()
                                .Add(Restrictions.Eq("Type", OperatorInterruptionType.TargetDate))
                                .Add(Restrictions.Eq("TargetDate", PlanDate))))

                         .List<OperatorInterruption>()
                         .Select(i => new TimeInterval(i.StartTime, i.FinishTime))
                         .ToArray());

                    transaction.Commit();
                }
            }

            return operatorInterruptions[queueOperator];
        }

        /// <summary>
        /// Получить расписание для услуги
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        public Schedule GetServiceSchedule(Service service)
        {
            if (!serviceSchedule.ContainsKey(service))
            {
                logger.Info("Загрузка расписания для услуги [{0}]", service);

                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    Schedule schedule = session.CreateCriteria<ServiceExceptionSchedule>()
                         .Add(Restrictions.Eq("Service", service))
                         .Add(Restrictions.Eq("ScheduleDate", PlanDate))
                         .SetMaxResults(1)
                         .UniqueResult<ServiceExceptionSchedule>();
                    if (schedule == null)
                    {
                        schedule = session.CreateCriteria<DefaultExceptionSchedule>()
                            .Add(Restrictions.Eq("ScheduleDate", PlanDate))
                            .UniqueResult<DefaultExceptionSchedule>();
                        if (schedule == null)
                        {
                            var dayOfWeek = PlanDate.DayOfWeek;

                            schedule = session.CreateCriteria<ServiceWeekdaySchedule>()
                                .Add(Restrictions.Eq("Service", service))
                                .Add(Restrictions.Eq("DayOfWeek", dayOfWeek))
                                .SetMaxResults(1)
                                .UniqueResult<ServiceWeekdaySchedule>();
                            if (schedule == null)
                            {
                                schedule = session.CreateCriteria<DefaultWeekdaySchedule>()
                                    .Add(Restrictions.Eq("DayOfWeek", dayOfWeek))
                                    .UniqueResult<DefaultWeekdaySchedule>();
                            }
                        }
                    }

                    if (schedule == null)
                    {
                        throw new Exception("Не удалось определить расписание для услуги");
                    }

                    serviceSchedule.Add(service, schedule);
                    transaction.Commit();
                }
            }

            return serviceSchedule[service];
        }

        /// <summary>
        /// Загрузить план очереди
        /// </summary>
        public void Load(DateTime planDate)
        {
            logger.Debug("Загрузка плана очереди на дату [{0}]", planDate);

            PlanDate = planDate.Date;
            PlanTime = TimeSpan.Zero;
            Version = 0;

            Refresh();
        }

        /// <summary>
        /// Поместить объект в хранилище
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public void Put(Entity entity)
        {
            logger.Info("Объект помещен в хранилище {0}", entity);
            storage.Put(entity);
        }

        public void Refresh()
        {
            storage.Clear();

            Flush(QueuePlanFlushMode.Full);

            using (var session = SessionProvider.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                logger.Info("Загрузка операторов");

                var operators = session.CreateCriteria<Operator>()
                    .AddOrder(Order.Asc("Surname"))
                    .AddOrder(Order.Asc("Name"))
                    .AddOrder(Order.Asc("Patronymic"))
                    .List<Operator>();

                OperatorsPlans.Clear();
                foreach (var o in operators)
                {
                    OperatorsPlans.Add(new OperatorPlan(storage.Put(o)));
                    logger.Debug("Загружен оператор [{0}]", o);
                }

                logger.Info("Загрузка запросов");

                var openedClientRequests = session.CreateCriteria<ClientRequest>()
                    .Add(Restrictions.Eq("RequestDate", PlanDate))
                    .Add(Restrictions.Eq("IsClosed", false))
                    .AddOrder(Order.Asc("Number"))
                    .List<ClientRequest>();

                clientRequests.Clear();
                foreach (var r in openedClientRequests)
                {
                    clientRequests.Add(storage.Put(r));
                    logger.Debug("Загружен [{0}] запрос", r.Number);
                }

                LastNumber = session.CreateCriteria<ClientRequest>()
                    .Add(Restrictions.Eq("RequestDate", PlanDate))
                    .SetProjection(Projections.Max("Number"))
                    .UniqueResult<int>();

                NotDistributedClientRequests.Clear();
                transaction.Commit();
            }
        }
    }
}