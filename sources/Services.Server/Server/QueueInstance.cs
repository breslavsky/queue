using AutoMapper;
using Junte.Data.NHibernate;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Criterion;
using NHibernate.SqlCommand;
using NLog;
using Queue.Common;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.IO;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Queue.Services.Server
{
    public class QueueInstance
    {
        #region dependency

        [Dependency]
        public ISessionProvider SessionProvider { get; set; }

        #endregion dependency

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const long OperatorHasGoneTimerInterval = TicksInterval._30Seconds;
        private const long TodayQueuePlanBuildInterval = TicksInterval._10Seconds;
        private const long TodayQueuePlanLoadInterval = TicksInterval._1Minute;

        private readonly Timer operatorHasGoneTimer;
        private readonly Timer todayQueuePlanBuildTimer;
        private readonly Timer todayQueuePlanLoadTimer;

        public QueueInstance()
        {
            logger.Debug("Создание экземпляра очереди");

            TodayQueuePlan = new QueuePlan();
            TodayQueuePlan.Load(DateTime.Today);

            TodayQueuePlan.OnCurrentClientRequestPlanUpdated += TodayQueuePlan_CurrentClientRequestPlanUpdated;
            TodayQueuePlan.OnOperatorPlanMetricsUpdated += TodayQueuePlan_OnOperatorPlanMetricsUpdated;

            todayQueuePlanLoadTimer = new Timer();
            todayQueuePlanLoadTimer.Elapsed += todayQueuePlanLoadTimer_Elapsed;
            todayQueuePlanLoadTimer.Start();

            todayQueuePlanBuildTimer = new Timer();
            todayQueuePlanBuildTimer.Elapsed += todayQueuePlanBuildTimer_Elapsed;
            todayQueuePlanBuildTimer.Start();

            operatorHasGoneTimer = new Timer();
            operatorHasGoneTimer.Elapsed += operatorHasGoneTimer_Elapsed;
            operatorHasGoneTimer.Start();
        }

        public event EventHandler<QueueInstanceEventArgs> OnCallClient;

        public event EventHandler<QueueInstanceEventArgs> OnClientRequestUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnConfigUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnCurrentClientRequestPlanUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnEvent;

        public event EventHandler<QueueInstanceEventArgs> OnOperatorPlanMetricsUpdated;

        public QueuePlan TodayQueuePlan { get; private set; }

        public void CallClient(ClientRequest clientRequest)
        {
            if (OnCallClient != null)
            {
                logger.Debug("Запуск обработчика для события [ClientCalling] с кол-вом слушателей [{0}]",
                    OnCallClient.GetInvocationList().Length);
                OnCallClient(this, new QueueInstanceEventArgs()
                {
                    ClientRequest = Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest)
                });
            }
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
            if (OnClientRequestUpdated != null)
            {
                logger.Debug("Запуск обработчика для события [OnClientRequestUpdated] с кол-вом слушателей [{0}]",
                    OnClientRequestUpdated.GetInvocationList().Length);
                OnClientRequestUpdated(this, new QueueInstanceEventArgs()
                {
                    ClientRequest = Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest)
                });
            }
        }

        public void ConfigUpdated(Config config)
        {
            if (OnConfigUpdated != null)
            {
                logger.Debug("Запуск обработчика для события [ConfigUpdated] с кол-вом слушателей [{0}]",
                    OnConfigUpdated.GetInvocationList().Length);
                OnConfigUpdated(this, new QueueInstanceEventArgs()
                {
                    Config = Mapper.Map<Config, DTO.Config>(config)
                });
            }
        }

        public void Dispose()
        {
            logger.Debug("Уничтожение экземпляра очереди");

            todayQueuePlanBuildTimer.Stop();

            TodayQueuePlan.OnCurrentClientRequestPlanUpdated -= TodayQueuePlan_CurrentClientRequestPlanUpdated;
            TodayQueuePlan.OnOperatorPlanMetricsUpdated -= TodayQueuePlan_OnOperatorPlanMetricsUpdated;
            TodayQueuePlan.Dispose();
        }

        public void Event(Event queueEvent)
        {
            if (OnEvent != null)
            {
                logger.Debug("Запуск обработчика для события [OnQueueEvent] с кол-вом слушателей [{0}]",
                    OnEvent.GetInvocationList().Length);
                OnEvent(this, new QueueInstanceEventArgs()
                {
                    Event = Mapper.Map<Event, DTO.Event>(queueEvent)
                });
            }
        }

        private void operatorHasGoneTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            operatorHasGoneTimer.Stop();
            if (operatorHasGoneTimer.Interval < OperatorHasGoneTimerInterval)
            {
                operatorHasGoneTimer.Interval = OperatorHasGoneTimerInterval;
            }

            try
            {
                using (var session = SessionProvider.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var clientsRequests = session.CreateCriteria<ClientRequest>("r")
                        .Add(Restrictions.Eq("RequestDate", DateTime.Today))
                        .Add(new Disjunction()
                            .Add(Restrictions.Eq("State", ClientRequestState.Calling))
                            .Add(Restrictions.Eq("State", ClientRequestState.Rendering)))
                        .Add(Restrictions.IsNotNull("Operator"))
                        .CreateCriteria("r.Operator", JoinType.InnerJoin)
                        .Add(Restrictions.Lt("Heartbeat", DateTime.Now - TimeSpan.FromSeconds(User.GoneTimeout)))
                        .List<ClientRequest>();

                    if (clientsRequests.Count > 0)
                    {
                        foreach (var r in clientsRequests)
                        {
                            switch (r.State)
                            {
                                case ClientRequestState.Calling:
                                    r.Return();
                                    session.Save(new ClientRequestEvent()
                                    {
                                        ClientRequest = r,
                                        Message = "Запрос был возвращен в очередь из-за потери связи с оператором"
                                    });
                                    break;

                                case ClientRequestState.Rendering:
                                    if (r.RenderStartTime < DateTime.Now.TimeOfDay - r.ClientInterval)
                                    {
                                        r.Rendered();
                                        session.Save(new ClientRequestEvent()
                                        {
                                            ClientRequest = r,
                                            Message = "Запрос был принудительно закрыт из-за потери связи с оператором"
                                        });
                                    }

                                    break;
                            }

                            session.Save(r);
                        }

                        using (var locker = TodayQueuePlan.WriteLock())
                        {
                            foreach (var r in clientsRequests)
                            {
                                TodayQueuePlan.Put(r);
                                ClientRequestUpdated(r);
                            }

                            transaction.Commit();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
            }
            finally
            {
                operatorHasGoneTimer.Start();
            }
        }

        private void TodayQueuePlan_CurrentClientRequestPlanUpdated(object sender, QueuePlanEventArgs e)
        {
            if (OnCurrentClientRequestPlanUpdated != null)
            {
                var eventArgs = new QueueInstanceEventArgs()
                {
                    Operator = Mapper.Map<Operator, DTO.Operator>(e.Operator)
                };

                if (e.ClientRequestPlan != null)
                {
                    logger.Debug("Текущий план запроса клиента у оператора [{0}] [{1}]", e.Operator, e.ClientRequestPlan);
                    eventArgs.ClientRequestPlan = Mapper.Map<ClientRequestPlan, DTO.ClientRequestPlan>(e.ClientRequestPlan);
                }
                else
                {
                    logger.Debug("У оператора [{0}] отсутствуют текущие запросы", e.Operator);
                }

                logger.Debug("Запуск обработчика для события [CurrentClientRequestPlanUpdated] с кол-вом слушателей [{0}]",
                    OnCurrentClientRequestPlanUpdated.GetInvocationList().Length);
                OnCurrentClientRequestPlanUpdated(this, eventArgs);
            }
        }

        private void TodayQueuePlan_OnBuilded(object sender, QueuePlanEventArgs e)
        {
            try
            {
                string directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "queue");
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string file = Path.Combine(directory, string.Format("queue-plan-{0:00000}.txt", TodayQueuePlan.Version));
                File.WriteAllLines(file, TodayQueuePlan.Report);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        private void TodayQueuePlan_OnOperatorPlanMetricsUpdated(object sender, QueuePlanEventArgs e)
        {
            if (OnOperatorPlanMetricsUpdated != null)
            {
                logger.Debug("Запуск обработчика для события [OperatorPlanMetricsUpdated] с кол-вом слушателей [{0}]",
                    OnOperatorPlanMetricsUpdated.GetInvocationList().Length);
                OnOperatorPlanMetricsUpdated(this, new QueueInstanceEventArgs()
                {
                    OperatorPlanMetrics = Mapper.Map<OperatorPlanMetrics, DTO.OperatorPlanMetrics>(e.OperatorPlanMetrics)
                });
            }
        }

        private void todayQueuePlanBuildTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            todayQueuePlanBuildTimer.Stop();
            if (todayQueuePlanBuildTimer.Interval < TodayQueuePlanBuildInterval)
            {
                todayQueuePlanBuildTimer.Interval = TodayQueuePlanBuildInterval;
            }

            logger.Info("Запуск построения плана очереди");

            try
            {
                if (DateTime.Now.TimeOfDay - TodayQueuePlan.PlanTime >
                    new TimeSpan(TodayQueuePlanBuildInterval))
                {
                    using (var locker = TodayQueuePlan.WriteLock())
                    {
                        TodayQueuePlan.Build(DateTime.Now.TimeOfDay);
#if THREADING
                        Thread.Sleep(2000);
#endif
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
            }
            finally
            {
                todayQueuePlanBuildTimer.Start();
            }
        }

        private void todayQueuePlanLoadTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            todayQueuePlanLoadTimer.Stop();
            if (todayQueuePlanLoadTimer.Interval < TodayQueuePlanLoadInterval)
            {
                todayQueuePlanLoadTimer.Interval = TodayQueuePlanLoadInterval;
            }

            logger.Info("Запуск смены даты плана очереди");

            try
            {
                if (TodayQueuePlan.PlanDate != DateTime.Today)
                {
                    using (var session = SessionProvider.OpenSession())
                    using (var transaction = session.BeginTransaction())
                    using (var locker = TodayQueuePlan.WriteLock(TimeSpan.FromMinutes(5)))
                    {
                        var opened = TodayQueuePlan.OperatorsPlans
                            .SelectMany(o => o.ClientRequestPlans
                                .Select(p => p.ClientRequest));

                        foreach (var r in opened)
                        {
                            var clientRequest = session.Merge(r);

                            try
                            {
                                clientRequest.Close(ClientRequestState.Absence);
                                clientRequest.Version++;
                                session.Save(clientRequest);
                            }
                            catch (Exception exception)
                            {
                                logger.Error(exception);
                            }
                        }

                        transaction.Commit();

                        TodayQueuePlan.Load(DateTime.Today);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
            }
            finally
            {
                todayQueuePlanLoadTimer.Start();
            }
        }
    }

    public class QueueInstanceEventArgs
    {
        public DTO.ClientRequest ClientRequest;
        public DTO.ClientRequestPlan ClientRequestPlan;
        public DTO.Config Config;
        public DTO.Event Event;
        public DTO.Operator Operator;
        public DTO.OperatorPlanMetrics OperatorPlanMetrics;
        public DTO.QueuePlan QueuePlan;
    }
}