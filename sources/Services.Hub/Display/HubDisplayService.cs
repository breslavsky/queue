using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Queue.Services.Hub
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class HubDisplayService : DependencyService, IHubDisplayService
    {
        #region dependency

        [Dependency]
        public IHubDisplayDriver[] Drivers { get; set; }

        #endregion dependency

        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IContextChannel channel;

        #endregion fields

        public HubDisplayService()
            : base()
        {
            logger.Debug("Создан новый экземпляр службы");

            channel = OperationContext.Current.Channel;
            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;

#if DEBUG
            Thread.Sleep(1000);
#endif
        }

        public async Task Heartbeat()
        {
            await Task.Run(() => DateTime.Now);
        }

        public async Task<string> Echo(string message)
        {
            return await Task.Run(() => message);
        }

        public async Task ShowText(byte deviceId, string text)
        {
            await Task.Run(() =>
            {
                foreach (var d in Drivers)
                {
                    d.ShowText(deviceId, text);
                }
            });
        }

        public async Task ClearText(byte deviceId)
        {
            await Task.Run(() =>
            {
                foreach (var d in Drivers)
                {
                    d.ClearText(deviceId);
                }
            });
        }

        public async Task<string[]> GetDrivers()
        {
            return await Task.Run(() =>
            {
                var drivers = new List<string>();
                foreach (var d in Drivers)
                {
                    drivers.Add(d.GetType().FullName);
                }
                return drivers.ToArray();
            });
        }

        #region channel

        private void channel_Closing(object sender, EventArgs e)
        {
            logger.Info("Канал службы закрывается");
        }

        private void channel_Faulted(object sender, EventArgs e)
        {
            logger.Info("В канале службы произошла ошибка");
        }

        #endregion channel
    }
}