using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class QueuePlanTcpService : QueuePlanService, IQueuePlanTcpService
    {
        private class Subscribtion
        {
            public QueuePlanSubscribtionArgs Args { get; set; }

            public EventHandler<QueueInstanceEventArgs> EventHandler { get; set; }
        }

        private readonly IDictionary<QueuePlanEventType, Subscribtion> subscriptions =
            new Dictionary<QueuePlanEventType, Subscribtion>();

        private readonly IQueuePlanCallback eventsCallback;

        public QueuePlanTcpService()
            : base()
        {
            try
            {
                eventsCallback = OperationContext.Current.GetCallbackChannel<IQueuePlanCallback>();
            }

            catch
            {
                logger.Debug("Не удалось получить канал обратного вызова [{0}]", sessionId);
            }

            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;
        }

        public bool IsSubscribed(QueuePlanEventType eventType)
        {
            return subscriptions.ContainsKey(eventType);
        }

        public void Subscribe(QueuePlanEventType eventType, QueuePlanSubscribtionArgs args = null)
        {
            if (!IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug("Подписка на событие [{0}] для [{1}]", eventType, sessionId);

                    switch (eventType)
                    {
                        case QueuePlanEventType.CallClient:
                            QueueInstance.OnCallClient += queueInstance_OnCallClient;

                            subscriptions[eventType] = new Subscribtion()

                            {
                                EventHandler = queueInstance_OnCallClient,
                                Args = args
                            };
                            break;

                        case QueuePlanEventType.ClientRequestUpdated:
                            QueueInstance.OnClientRequestUpdated += queueInstance_OnClientRequestUpdated;

                            subscriptions[eventType] = new Subscribtion()

                            {
                                EventHandler = queueInstance_OnClientRequestUpdated,
                                Args = args
                            };
                            break;

                        case QueuePlanEventType.CurrentClientRequestPlanUpdated:
                            QueueInstance.OnCurrentClientRequestPlanUpdated += queueInstance_OnCurrentClientRequestPlanUpdated;

                            subscriptions[eventType] = new Subscribtion()

                            {
                                EventHandler = queueInstance_OnCurrentClientRequestPlanUpdated,
                                Args = args
                            };
                            break;

                        case QueuePlanEventType.OperatorPlanMetricsUpdated:
                            QueueInstance.OnOperatorPlanMetricsUpdated += queueInstance_OnOperatorPlanMetricsUpdated;

                            subscriptions[eventType] = new Subscribtion()

                            {
                                EventHandler = queueInstance_OnOperatorPlanMetricsUpdated,
                                Args = args
                            };
                            break;

                        case QueuePlanEventType.ConfigUpdated:
                            QueueInstance.OnConfigUpdated += queueInstance_OnConfigUpdated;

                            subscriptions[eventType] = new Subscribtion()

                            {
                                EventHandler = queueInstance_OnConfigUpdated,
                                Args = args
                            };
                            break;

                        case QueuePlanEventType.Event:
                            QueueInstance.OnEvent += queueInstance_OnEvent;

                            subscriptions[eventType] = new Subscribtion()

                            {
                                EventHandler = queueInstance_OnEvent,
                                Args = args
                            };
                            break;
                    }
                }
            }
        }

        public void UnSubscribe(QueuePlanEventType eventType)
        {
            if (IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug("Отписка от события [{0}] для [{1}]", eventType, sessionId);

                    switch (eventType)
                    {
                        case QueuePlanEventType.CallClient:
                            QueueInstance.OnCallClient -= queueInstance_OnCallClient;
                            break;

                        case QueuePlanEventType.ClientRequestUpdated:
                            QueueInstance.OnClientRequestUpdated -= queueInstance_OnClientRequestUpdated;
                            break;

                        case QueuePlanEventType.CurrentClientRequestPlanUpdated:
                            QueueInstance.OnCurrentClientRequestPlanUpdated -= queueInstance_OnCurrentClientRequestPlanUpdated;
                            break;

                        case QueuePlanEventType.OperatorPlanMetricsUpdated:
                            QueueInstance.OnOperatorPlanMetricsUpdated -= queueInstance_OnOperatorPlanMetricsUpdated;
                            break;

                        case QueuePlanEventType.ConfigUpdated:
                            QueueInstance.OnConfigUpdated -= queueInstance_OnConfigUpdated;
                            break;

                        case QueuePlanEventType.Event:
                            QueueInstance.OnEvent -= queueInstance_OnEvent;
                            break;
                    }

                    subscriptions.Remove(eventType);
                }
            }
        }

        public void UnSubscribe()
        {
            logger.Info("Экземпляр службы уничтожен [{0}]", sessionId);
            try
            {
                var eventTypes = subscriptions.Keys.ToArray();
                foreach (var t in eventTypes)
                {
                    UnSubscribe(t);
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        private void queueInstance_OnCallClient(object sender, QueueInstanceEventArgs e)
        {
            try
            {
                Task.Run(() => eventsCallback.CallClient(e.ClientRequest));
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(QueuePlanEventType.CallClient);
            }
        }

        private void queueInstance_OnClientRequestUpdated(object sender, QueueInstanceEventArgs e)
        {
            try
            {
                Task.Run(() => eventsCallback.ClientRequestUpdated(e.ClientRequest));
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(QueuePlanEventType.ClientRequestUpdated);
            }
        }

        private void queueInstance_OnConfigUpdated(object sender, QueueInstanceEventArgs e)
        {
            var subscription = subscriptions[QueuePlanEventType.ConfigUpdated];
            var args = subscription.Args;

            if (args == null || args.ConfigTypes.Length > 0
                && args.ConfigTypes.Contains(e.Config.Type))
            {
                try
                {
                    Task.Run(() => eventsCallback.ConfigUpdated(e.Config));
                }
                catch (ObjectDisposedException exception)
                {
                    logger.Debug(exception);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    UnSubscribe(QueuePlanEventType.ConfigUpdated);
                }
            }
        }

        private void queueInstance_OnCurrentClientRequestPlanUpdated(object sender, QueueInstanceEventArgs e)
        {
            var subscription = subscriptions[QueuePlanEventType.CurrentClientRequestPlanUpdated];
            var args = subscription.Args;

            if (args == null || args.Operators != null && args.Operators.Any(o => o.Equals(e.Operator)))
            {
                try
                {
                    Task.Run(() => eventsCallback.CurrentClientRequestPlanUpdated(e.ClientRequestPlan, e.Operator));
                }
                catch (ObjectDisposedException exception)
                {
                    logger.Debug(exception);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    UnSubscribe(QueuePlanEventType.CurrentClientRequestPlanUpdated);
                }
            }
        }

        private void queueInstance_OnEvent(object sender, QueueInstanceEventArgs e)
        {
            try
            {
                Task.Run(() => eventsCallback.Event(e.Event));
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(QueuePlanEventType.Event);
            }
        }

        private void queueInstance_OnOperatorPlanMetricsUpdated(object sender, QueueInstanceEventArgs e)
        {
            var subscription = subscriptions[QueuePlanEventType.OperatorPlanMetricsUpdated];
            var args = subscription.Args;

            logger.Debug("OnOperatorPlanMetricsUpdated = {0}", args.Operators);

            if (args == null || args.Operators != null && args.Operators.Any(o => o.Equals(e.OperatorPlanMetrics.Operator)))
            {
                try
                {
                    Task.Run(() => eventsCallback.OperatorPlanMetricsUpdated(e.OperatorPlanMetrics));
                }
                catch (ObjectDisposedException exception)
                {
                    logger.Debug(exception);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    UnSubscribe(QueuePlanEventType.OperatorPlanMetricsUpdated);
                }
            }
        }

        private void channel_Closing(object sender, EventArgs e)
        {
            UnSubscribe();
        }

        private void channel_Faulted(object sender, EventArgs e)
        {
            UnSubscribe();
        }
    }
}