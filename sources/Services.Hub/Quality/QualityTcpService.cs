using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Hub;
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
    public class QualityTcpService : QualityService, IQualityTcpService
    {
        private class Subscribtion
        {
            public QualityServiceSubscribtionArgs Args { get; set; }

            public EventHandler EventHandler { get; set; }
        }

        #region fields

        private readonly IQualityCallback eventsCallback;

        private readonly IDictionary<QualityServiceEventType, Subscribtion> subscriptions =
            new Dictionary<QualityServiceEventType, Subscribtion>();

        #endregion fields

        public QualityTcpService()
            : base()
        {
            try
            {
                eventsCallback = OperationContext.Current.GetCallbackChannel<IQualityCallback>();
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

        public bool IsSubscribed(QualityServiceEventType eventType)
        {
            return subscriptions.ContainsKey(eventType);
        }

        public void Subscribe(QualityServiceEventType eventType, QualityServiceSubscribtionArgs args = null)
        {
            if (!IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug("Подписка на событие [{0}]", eventType);

                    switch (eventType)
                    {
                        case QualityServiceEventType.RatingAccepted:

                            foreach (var d in Drivers)
                            {
                                d.Accepted += driver_Accepted;
                            }

                            break;
                    }
                }
            }
        }

        public void UnSubscribe(QualityServiceEventType eventType)
        {
            if (IsSubscribed(eventType))
            {
                lock (subscriptions)
                {
                    logger.Debug("Отписка от события [{0}]", eventType);

                    switch (eventType)
                    {
                        case QualityServiceEventType.RatingAccepted:

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

        public override async Task Disable(byte deviceId)
        {
            await base.Enable(deviceId);
            await Task.Run(() =>
            {
                foreach (var d in Drivers)
                {
                    d.Disable(deviceId);
                    d.Accepted -= driver_Accepted;
                }
            });
        }

        private void driver_Accepted(object sender, IQualityDriverArgs e)
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
                UnSubscribe(QualityServiceEventType.RatingAccepted);
            }
        }
    }
}