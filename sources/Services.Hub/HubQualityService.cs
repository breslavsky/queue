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
    public partial class HubQualityService : IHubQualityService, IHubQualityHttpService
    {
        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IContextChannel channel;
        private IHubQualityCallback eventsCallback;
        private IDictionary<HubQualityServiceEventType, Subscribtion> subscriptions;
        private IList<IHubQualityDriver> drivers;

        #endregion fields

        public HubQualityService()
        {
            logger.Debug("Создан новый экземпляр службы");

            try
            {
                eventsCallback = OperationContext.Current.GetCallbackChannel<IHubQualityCallback>();
            }
            catch
            {
                logger.Debug("Не удалось получить канал обратного вызова");
            }

            subscriptions = new Dictionary<HubQualityServiceEventType, Subscribtion>();

            channel = OperationContext.Current.Channel;
            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;

#if DEBUG
            Thread.Sleep(1000);
#endif
        }

        public async Task<string> Echo(string message)
        {
            return await Task.Run(() => message);
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
                        case HubQualityServiceEventType.RatingAccepted:

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
                        case HubQualityServiceEventType.RatingAccepted:

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
            logger.Info("Канал службы закрывается");

            UnSubscribe();
        }

        private void channel_Faulted(object sender, EventArgs e)
        {
            logger.Info("В канале службы произошла ошибка");

            UnSubscribe();
        }

        #endregion channel

        public class Subscribtion
        {
            public HubQualityServiceSubscribtionArgs Args { get; set; }

            public EventHandler EventHandler { get; set; }
        }
    }
}