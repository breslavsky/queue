using AutoMapper;
using Junte.Data.NHibernate;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.IO;
using System.Linq;
using System.Timers;
using Timer = System.Timers.Timer;

namespace Queue.Services.Server
{
    public class QueueInstanceEventArgs
    {
        public DTO.QueuePlan QueuePlan;
        public DTO.ClientRequestPlan ClientRequestPlan;
        public DTO.ClientRequest ClientRequest;
        public DTO.Operator Operator;
        public DTO.OperatorPlanMetrics OperatorPlanMetrics;
        public DTO.Config Config;
        public DTO.Event Event;
    }

    public class QueueInstance : IQueueInstance
    {
        private const int TodayQueuePlanBuildInterval = 10000;
        private static readonly ILog logger = LogManager.GetLogger(typeof(QueueInstance));

        private Timer todayQueuePlanTimer;

        public QueueInstance()
        {
            logger.Debug("Создание экземпляра очереди");

            TodayQueuePlan = new QueuePlan();

            TodayQueuePlan.OnCurrentClientRequestPlanUpdated += TodayQueuePlan_CurrentClientRequestPlanUpdated;
            TodayQueuePlan.OnOperatorPlanMetricsUpdated += TodayQueuePlan_OnOperatorPlanMetricsUpdated;

            todayQueuePlanTimer = new Timer();
            todayQueuePlanTimer.Elapsed += todayQueuePlanTimer_Elapsed;
            todayQueuePlanTimer.Start();
        }

        public event EventHandler<QueueInstanceEventArgs> OnCallClient;

        public event EventHandler<QueueInstanceEventArgs> OnClientRequestUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnCurrentClientRequestPlanUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnOperatorPlanMetricsUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnConfigUpdated;

        public event EventHandler<QueueInstanceEventArgs> OnEvent;

        public QueuePlan TodayQueuePlan { get; private set; }

        private ISessionProvider sessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public void CallClient(ClientRequest clientRequest)
        {
            if (OnCallClient != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [ClientCalling] с кол-вом слушателей [{0}]", OnCallClient.GetInvocationList().Length));
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
                logger.Debug(string.Format("Запуск обработчика для события [OnClientRequestUpdated] с кол-вом слушателей [{0}]", OnClientRequestUpdated.GetInvocationList().Length));
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
                logger.Debug(string.Format("Запуск обработчика для события [ConfigUpdated] с кол-вом слушателей [{0}]", OnConfigUpdated.GetInvocationList().Length));
                OnConfigUpdated(this, new QueueInstanceEventArgs()
                {
                    Config = Mapper.Map<Config, DTO.Config>(config)
                });
            }
        }

        public void Event(Event queueEvent)
        {
            if (OnEvent != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [OnQueueEvent] с кол-вом слушателей [{0}]", OnEvent.GetInvocationList().Length));
                OnEvent(this, new QueueInstanceEventArgs()
                {
                    Event = Mapper.Map<Event, DTO.Event>(queueEvent)
                });
            }
        }

        public void Dispose()
        {
            logger.Debug("Уничтожение экземпляра очереди");

            todayQueuePlanTimer.Stop();

            TodayQueuePlan.OnCurrentClientRequestPlanUpdated -= TodayQueuePlan_CurrentClientRequestPlanUpdated;
            TodayQueuePlan.OnOperatorPlanMetricsUpdated -= TodayQueuePlan_OnOperatorPlanMetricsUpdated;
            TodayQueuePlan.Dispose();
        }

        private void todayQueuePlanTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            todayQueuePlanTimer.Stop();
            if (todayQueuePlanTimer.Interval < TodayQueuePlanBuildInterval)
            {
                todayQueuePlanTimer.Interval = TodayQueuePlanBuildInterval;
            }

            try
            {
                if (TodayQueuePlan.PlanDate != DateTime.Today)
                {
                    using (var session = sessionProvider.OpenSession())
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

                if (DateTime.Now.TimeOfDay - TodayQueuePlan.PlanTime > new TimeSpan(TodayQueuePlanBuildInterval))
                {
                    using (var locker = TodayQueuePlan.WriteLock())
                    {
                        TodayQueuePlan.Build(DateTime.Now.TimeOfDay);
                        //Thread.Sleep(2000);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Fatal(exception);
            }
            finally
            {
                todayQueuePlanTimer.Start();
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
                    logger.Debug(string.Format("Текущий план запроса клиента у оператора [{0}] [{1}]", e.Operator, e.ClientRequestPlan));
                    eventArgs.ClientRequestPlan = Mapper.Map<ClientRequestPlan, DTO.ClientRequestPlan>(e.ClientRequestPlan);
                }
                else
                {
                    logger.Debug(string.Format("У оператора [{0}] отсутствуют текущие запросы", e.Operator));
                }

                logger.Debug(string.Format("Запуск обработчика для события [CurrentClientRequestPlanUpdated] с кол-вом слушателей [{0}]", OnCurrentClientRequestPlanUpdated.GetInvocationList().Length));
                OnCurrentClientRequestPlanUpdated(this, eventArgs);
            }
        }

        private void TodayQueuePlan_OnOperatorPlanMetricsUpdated(object sender, QueuePlanEventArgs e)
        {
            if (OnOperatorPlanMetricsUpdated != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [OperatorPlanMetricsUpdated] с кол-вом слушателей [{0}]", OnOperatorPlanMetricsUpdated.GetInvocationList().Length));
                OnOperatorPlanMetricsUpdated(this, new QueueInstanceEventArgs()
                {
                    OperatorPlanMetrics = Mapper.Map<OperatorPlanMetrics, DTO.OperatorPlanMetrics>(e.OperatorPlanMetrics)
                });
            }
        }
    }
}