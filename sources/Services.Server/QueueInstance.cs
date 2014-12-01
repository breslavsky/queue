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
    public delegate void QueueInstanceEventHandler(object sender, QueueInstanceEventArgs e);

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
        private const int TODAY_QUEUE_PLAN_BUILD_INTERVAL = 10000;
        private static readonly ILog logger = LogManager.GetLogger(typeof(QueueInstance));

        private Timer todayQueuePlanTimer;

        public QueueInstance()
        {
            logger.Debug("Создание экземпляра очереди");

            TodayQueuePlan = new QueuePlan();

            TodayQueuePlan.OnBuilded += TodayQueuePlan_OnBuilded;
            TodayQueuePlan.OnCurrentClientRequestPlanUpdated += TodayQueuePlan_CurrentClientRequestPlanUpdated;
            TodayQueuePlan.OnOperatorPlanMetricsUpdated += TodayQueuePlan_OnOperatorPlanMetricsUpdated;

            todayQueuePlanTimer = new Timer();
            todayQueuePlanTimer.Elapsed += todayQueuePlanTimer_Elapsed;
            todayQueuePlanTimer.Start();
        }

        public event QueueInstanceEventHandler OnCallClient
        {
            add { callClientHandler += value; }
            remove { callClientHandler -= value; }
        }

        public event QueueInstanceEventHandler OnTodayQueuePlanBuilded
        {
            add { onTodayQueuePlanBuildedHandler += value; }
            remove { onTodayQueuePlanBuildedHandler -= value; }
        }

        public event QueueInstanceEventHandler OnClientRequestUpdated
        {
            add { clientRequestUpdatedHandler += value; }
            remove { clientRequestUpdatedHandler -= value; }
        }

        public event QueueInstanceEventHandler OnCurrentClientRequestPlanUpdated
        {
            add { currentClientRequestPlanUpdatedHandler += value; }
            remove { currentClientRequestPlanUpdatedHandler -= value; }
        }

        public event QueueInstanceEventHandler OnOperatorPlanMetricsUpdated
        {
            add { operatorPlanMetricsUpdatedHandler += value; }
            remove { operatorPlanMetricsUpdatedHandler -= value; }
        }

        public event QueueInstanceEventHandler OnConfigUpdated
        {
            add { configUpdatedHandler += value; }
            remove { configUpdatedHandler -= value; }
        }

        public event QueueInstanceEventHandler OnEvent
        {
            add { eventHandler += value; }
            remove { eventHandler -= value; }
        }

        private event QueueInstanceEventHandler callClientHandler;

        private event QueueInstanceEventHandler onTodayQueuePlanBuildedHandler;

        private event QueueInstanceEventHandler clientRequestUpdatedHandler;

        private event QueueInstanceEventHandler currentClientRequestPlanUpdatedHandler;

        private event QueueInstanceEventHandler operatorPlanMetricsUpdatedHandler;

        private event QueueInstanceEventHandler configUpdatedHandler;

        private event QueueInstanceEventHandler eventHandler;

        public QueuePlan TodayQueuePlan { get; private set; }

        private ISessionProvider sessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public void CallClient(ClientRequest clientRequest)
        {
            if (callClientHandler != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [ClientCalling] с кол-вом слушателей [{0}]", callClientHandler.GetInvocationList().Length));
                callClientHandler(this, new QueueInstanceEventArgs()
                {
                    ClientRequest = Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest)
                });
            }
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
            if (clientRequestUpdatedHandler != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [OnClientRequestUpdated] с кол-вом слушателей [{0}]", clientRequestUpdatedHandler.GetInvocationList().Length));
                clientRequestUpdatedHandler(this, new QueueInstanceEventArgs()
                {
                    ClientRequest = Mapper.Map<ClientRequest, DTO.ClientRequest>(clientRequest)
                });
            }
        }

        public void ConfigUpdated(Config config)
        {
            if (configUpdatedHandler != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [ConfigUpdated] с кол-вом слушателей [{0}]", configUpdatedHandler.GetInvocationList().Length));
                configUpdatedHandler(this, new QueueInstanceEventArgs()
                {
                    Config = Mapper.Map<Config, DTO.Config>(config)
                });
            }
        }

        public void Event(Event queueEvent)
        {
            if (eventHandler != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [OnQueueEvent] с кол-вом слушателей [{0}]", eventHandler.GetInvocationList().Length));
                eventHandler(this, new QueueInstanceEventArgs()
                {
                    Event = Mapper.Map<Event, DTO.Event>(queueEvent)
                });
            }
        }

        public void Dispose()
        {
            logger.Debug("Уничтожение экземпляра очереди");

            todayQueuePlanTimer.Stop();

            TodayQueuePlan.OnBuilded -= TodayQueuePlan_OnBuilded;
            TodayQueuePlan.OnCurrentClientRequestPlanUpdated -= TodayQueuePlan_CurrentClientRequestPlanUpdated;
            TodayQueuePlan.OnOperatorPlanMetricsUpdated -= TodayQueuePlan_OnOperatorPlanMetricsUpdated;
            TodayQueuePlan.Dispose();
        }

        private void todayQueuePlanTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            todayQueuePlanTimer.Stop();
            if (todayQueuePlanTimer.Interval < TODAY_QUEUE_PLAN_BUILD_INTERVAL)
            {
                todayQueuePlanTimer.Interval = TODAY_QUEUE_PLAN_BUILD_INTERVAL;
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

                if (DateTime.Now.TimeOfDay - TodayQueuePlan.PlanTime > new TimeSpan(TODAY_QUEUE_PLAN_BUILD_INTERVAL))
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
            if (onTodayQueuePlanBuildedHandler != null)
            {
                var queuePlan = sender as QueuePlan;

                logger.Debug(string.Format("Запуск обработчика для события [TodayQueuePlanBuilded] с кол-вом слушателей [{0}]", onTodayQueuePlanBuildedHandler.GetInvocationList().Length));
                onTodayQueuePlanBuildedHandler(this, new QueueInstanceEventArgs()
                {
                    QueuePlan = Mapper.Map<QueuePlan, DTO.QueuePlan>(queuePlan)
                });
            }
        }

        private void TodayQueuePlan_CurrentClientRequestPlanUpdated(object sender, QueuePlanEventArgs e)
        {
            if (currentClientRequestPlanUpdatedHandler != null)
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

                logger.Debug(string.Format("Запуск обработчика для события [CurrentClientRequestPlanUpdated] с кол-вом слушателей [{0}]", currentClientRequestPlanUpdatedHandler.GetInvocationList().Length));
                currentClientRequestPlanUpdatedHandler(this, eventArgs);
            }
        }

        private void TodayQueuePlan_OnOperatorPlanMetricsUpdated(object sender, QueuePlanEventArgs e)
        {
            if (operatorPlanMetricsUpdatedHandler != null)
            {
                logger.Debug(string.Format("Запуск обработчика для события [OperatorPlanMetricsUpdated] с кол-вом слушателей [{0}]", operatorPlanMetricsUpdatedHandler.GetInvocationList().Length));
                operatorPlanMetricsUpdatedHandler(this, new QueueInstanceEventArgs()
                {
                    OperatorPlanMetrics = Mapper.Map<OperatorPlanMetrics, DTO.OperatorPlanMetrics>(e.OperatorPlanMetrics)
                });
            }
        }
    }
}