using Junte.Data.NHibernate;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Queue.Model;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true)]
    public partial class ServerService : IServerService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ServerService));

        private User currentUser;

        private IServerCallback eventsCallback;

        private IDictionary<ServerServiceEventType, Subscribtion> subscriptions;

        private string sessionId;

        private IContextChannel channel;

        public ServerService()
        {
            sessionId = OperationContext.Current.SessionId;
            logger.Debug(string.Format("Создан новый экземпляр службы [{0}]", sessionId));

            eventsCallback = OperationContext.Current.GetCallbackChannel<IServerCallback>();
            subscriptions = new Dictionary<ServerServiceEventType, Subscribtion>();

            channel = OperationContext.Current.Channel;
            channel.Faulted += Channel_Faulted;
            channel.Closing += Channel_Closing;
        }

        private IQueueInstance queueInstance
        {
            get { return ServiceLocator.Current.GetInstance<IQueueInstance>(); }
        }

        private ISessionProvider sessionProvider
        {
            get { return ServiceLocator.Current.GetInstance<ISessionProvider>(); }
        }

        public async Task<DateTime> GetDateTime()
        {
            return await Task.Run(() =>
            {
                return DateTime.Now;
            });
        }

        public bool IsSubscribed(ServerServiceEventType eventType)
        {
            return subscriptions.ContainsKey(eventType);
        }

        public void Subscribe(ServerServiceEventType eventType, ServerSubscribtionArgs args = null)
        {
            if (!IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug(string.Format("Подписка на событие [{0}] для [{1}]", eventType, sessionId));

                    switch (eventType)
                    {
                        case ServerServiceEventType.CallClient:
                            queueInstance.OnCallClient += queueInstance_OnCallClient;
                            subscriptions[eventType] = new Subscribtion()
                            {
                                EventHandler = queueInstance_OnCallClient,
                                Args = args
                            };
                            break;

                        case ServerServiceEventType.ClientRequestUpdated:
                            queueInstance.OnClientRequestUpdated += queueInstance_OnClientRequestUpdated;
                            subscriptions[eventType] = new Subscribtion()
                            {
                                EventHandler = queueInstance_OnClientRequestUpdated,
                                Args = args
                            };
                            break;

                        case ServerServiceEventType.CurrentClientRequestPlanUpdated:
                            queueInstance.OnCurrentClientRequestPlanUpdated += queueInstance_OnCurrentClientRequestPlanUpdated;
                            subscriptions[eventType] = new Subscribtion()
                            {
                                EventHandler = queueInstance_OnCurrentClientRequestPlanUpdated,
                                Args = args
                            };
                            break;

                        case ServerServiceEventType.OperatorPlanMetricsUpdated:
                            queueInstance.OnOperatorPlanMetricsUpdated += queueInstance_OnOperatorPlanMetricsUpdated;
                            subscriptions[eventType] = new Subscribtion()
                            {
                                EventHandler = queueInstance_OnOperatorPlanMetricsUpdated,
                                Args = args
                            };
                            break;

                        case ServerServiceEventType.ConfigUpdated:
                            queueInstance.OnConfigUpdated += queueInstance_OnConfigUpdated;
                            subscriptions[eventType] = new Subscribtion()
                            {
                                EventHandler = queueInstance_OnConfigUpdated,
                                Args = args
                            };
                            break;

                        case ServerServiceEventType.Event:
                            queueInstance.OnEvent += queueInstance_OnEvent;
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

        public void UnSubscribe(ServerServiceEventType eventType)
        {
            if (IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug(string.Format("Отписка от события [{0}] для [{1}]", eventType, sessionId));

                    switch (eventType)
                    {
                        case ServerServiceEventType.CallClient:
                            queueInstance.OnCallClient -= queueInstance_OnCallClient;
                            break;

                        case ServerServiceEventType.ClientRequestUpdated:
                            queueInstance.OnClientRequestUpdated -= queueInstance_OnClientRequestUpdated;
                            break;

                        case ServerServiceEventType.CurrentClientRequestPlanUpdated:
                            queueInstance.OnCurrentClientRequestPlanUpdated -= queueInstance_OnCurrentClientRequestPlanUpdated;
                            break;

                        case ServerServiceEventType.OperatorPlanMetricsUpdated:
                            queueInstance.OnOperatorPlanMetricsUpdated -= queueInstance_OnOperatorPlanMetricsUpdated;
                            break;

                        case ServerServiceEventType.ConfigUpdated:
                            queueInstance.OnConfigUpdated -= queueInstance_OnConfigUpdated;
                            break;

                        case ServerServiceEventType.Event:
                            queueInstance.OnEvent -= queueInstance_OnEvent;
                            break;
                    }

                    subscriptions.Remove(eventType);
                }
            }
        }

        public void UnSubscribe()
        {
            logger.Info(string.Format("Экземпляр службы уничтожен [{0}]", sessionId));
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

        private void checkPermission(UserRole role)
        {
            if (currentUser == null)
            {
                throw new FaultException("Пользователь не авторизован в системе");
            }

            if (currentUser is Administrator
                && role.HasFlag(UserRole.Administrator))
            {
                return;
            }

            if (currentUser is Manager
                && role.HasFlag(UserRole.Manager))
            {
                return;
            }

            if (currentUser is Operator
                && role.HasFlag(UserRole.Operator))
            {
                return;
            }

            throw new FaultException("Ошибка прав доступа");
        }

        private void queueInstance_OnCallClient(object sender, QueueInstanceEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    eventsCallback.CallClient(e.ClientRequest);
                });
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(ServerServiceEventType.CallClient);
            }
        }

        private void queueInstance_OnClientRequestUpdated(object sender, QueueInstanceEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    eventsCallback.ClientRequestUpdated(e.ClientRequest);
                });
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(ServerServiceEventType.ClientRequestUpdated);
            }
        }

        private void queueInstance_OnCurrentClientRequestPlanUpdated(object sender, QueueInstanceEventArgs e)
        {
            var subscription = subscriptions[ServerServiceEventType.CurrentClientRequestPlanUpdated];
            var args = subscription.Args;

            if (args == null || args.Operators != null && args.Operators.Any(o => o.Equals(e.Operator)))
            {
                try
                {
                    Task.Run(() =>
                    {
                        eventsCallback.CurrentClientRequestPlanUpdated(e.ClientRequestPlan, e.Operator);
                    });
                }
                catch (ObjectDisposedException exception)
                {
                    logger.Debug(exception);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    UnSubscribe(ServerServiceEventType.CurrentClientRequestPlanUpdated);
                }
            }
        }

        private void queueInstance_OnOperatorPlanMetricsUpdated(object sender, QueueInstanceEventArgs e)
        {
            var subscription = subscriptions[ServerServiceEventType.OperatorPlanMetricsUpdated];
            var args = subscription.Args;

            logger.DebugFormat("OnOperatorPlanMetricsUpdated = {0}", args.Operators);

            if (args == null || args.Operators != null && args.Operators.Any(o => o.Equals(e.OperatorPlanMetrics.Operator)))
            {
                try
                {
                    Task.Run(() =>
                    {
                        eventsCallback.OperatorPlanMetricsUpdated(e.OperatorPlanMetrics);
                    });
                }
                catch (ObjectDisposedException exception)
                {
                    logger.Debug(exception);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    UnSubscribe(ServerServiceEventType.OperatorPlanMetricsUpdated);
                }
            }
        }

        private void queueInstance_OnConfigUpdated(object sender, QueueInstanceEventArgs e)
        {
            var subscription = subscriptions[ServerServiceEventType.ConfigUpdated];
            var args = subscription.Args;

            if (args == null || args.ConfigTypes.Length > 0
                && args.ConfigTypes.Contains(e.Config.Type))
            {
                try
                {
                    Task.Run(() =>
                    {
                        eventsCallback.ConfigUpdated(e.Config);
                    });
                }
                catch (ObjectDisposedException exception)
                {
                    logger.Debug(exception);
                }
                catch (Exception exception)
                {
                    logger.Error(exception);
                    UnSubscribe(ServerServiceEventType.ConfigUpdated);
                }
            }
        }

        private void queueInstance_OnEvent(object sender, QueueInstanceEventArgs e)
        {
            try
            {
                Task.Run(() =>
                {
                    eventsCallback.Event(e.Event);
                });
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(ServerServiceEventType.Event);
            }
        }

        private void Channel_Faulted(object sender, EventArgs e)
        {
            logger.Info(string.Format("В канале службы произошла ошибка [{0}]", sessionId));

            UnSubscribe();
        }

        private void Channel_Closing(object sender, EventArgs e)
        {
            logger.Info(string.Format("Канал службы закрывается [{0}]", sessionId));

            UnSubscribe();
        }

        public class Subscribtion
        {
            public QueueInstanceEventHandler EventHandler { get; set; }

            public ServerSubscribtionArgs Args { get; set; }
        }
    }
}