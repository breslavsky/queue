using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Queue.Services.Hub
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class HubQualityTcpService : HubQualityService, IHubQualityTcpService
    {
        public class Subscribtion
        {
            public HubQualityServiceSubscribtionArgs Args { get; set; }

            public EventHandler EventHandler { get; set; }
        }

        #region fields

        private readonly IHubQualityCallback eventsCallback;

        private readonly IDictionary<HubQualityServiceEventType, Subscribtion> subscriptions =
            new Dictionary<HubQualityServiceEventType, Subscribtion>();

        #endregion fields

        public HubQualityTcpService()
            : base()
        {
            try
            {
                eventsCallback = OperationContext.Current.GetCallbackChannel<IHubQualityCallback>();
            }
            catch
            {
                logger.Debug("Не удалось получить канал обратного вызова");
            }

            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;

#if DEBUG
            Thread.Sleep(1000);
#endif
        }

        #region subscription

        public bool IsSubscribed(HubQualityServiceEventType eventType)
        {
            return subscriptions.ContainsKey(eventType);
        }

        public void Subscribe(HubQualityServiceEventType eventType, HubQualityServiceSubscribtionArgs args = null)
        {
            if (!IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug("Подписка на событие [{0}]", eventType);

                    switch (eventType)
                    {
                        case HubQualityServiceEventType.Accepted:

                            foreach (var d in Drivers)
                            {
                                d.Accepted += driver_Accepted;
                            }

                            break;
                    }
                }
            }
        }

        public void UnSubscribe(HubQualityServiceEventType eventType)
        {
            if (IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug("Отписка от события [{0}]", eventType);

                    switch (eventType)
                    {
                        case HubQualityServiceEventType.Accepted:

                            foreach (var d in Drivers)
                            {
                                d.Accepted -= driver_Accepted;
                            }

                            break;
                    }

                    subscriptions.Remove(eventType);
                }
            }
        }

        public void UnSubscribe()
        {
            logger.Info("Экземпляр службы уничтожен");
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

        #endregion subscription

        #region channel

        private void channel_Closing(object sender, EventArgs e)
        {
            UnSubscribe();
        }

        private void channel_Faulted(object sender, EventArgs e)
        {
            UnSubscribe();
        }

        #endregion channel

        public override async Task Enable(byte deviceId)
        {
            await base.Enable(deviceId);
            await Task.Run(() =>
            {
                foreach (var d in Drivers)
                {
                    d.Enable(deviceId);
                    d.Accepted += driver_Accepted;
                }
            });
        }

        private void driver_Accepted(object sender, IHubQualityDriverArgs e)
        {
            try
            {
                Task.Run(() => eventsCallback.Accepted(e.Rating));
            }
            catch (ObjectDisposedException exception)
            {
                logger.Debug(exception);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                UnSubscribe(HubQualityServiceEventType.Accepted);
            }
        }
    }
}