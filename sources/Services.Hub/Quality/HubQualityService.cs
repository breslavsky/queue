using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Hub
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class HubQualityService : DependencyService, IHubQualityService
    {
        #region dependency

        [Dependency]
        public IHubQualityDriver[] Drivers { get; set; }

        #endregion dependency

        #region fields

        protected readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly IContextChannel channel;

        #endregion fields

        public HubQualityService()
            : base()
        {
            logger.Debug("Создан новый экземпляр службы");

            channel = OperationContext.Current.Channel;
            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;
        }

        public async Task Heartbeat()
        {
            await Task.Run(() => DateTime.Now);
        }

        public async Task<string> Echo(string message)
        {
            return await Task.Run(() => message);
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

        public virtual async Task Enable(byte deviceId)
        {
            await Task.Run(() =>
            {
                foreach (var d in Drivers)
                {
                    d.Enable(deviceId);
                }
            });
        }

        public virtual async Task Disable(byte deviceId)
        {
            await Task.Run(() =>
            {
                foreach (var d in Drivers)
                {
                    d.Disable(deviceId);
                }
            });
        }

        public async Task<Dictionary<byte, byte>> GetAnswers()
        {
            return await Task.Run(() =>
            {
                var answers = new Dictionary<byte, byte>();
                foreach (var d in Drivers)
                {
                    answers = answers.Union(d.Answers).ToDictionary(x => x.Key, x => x.Value);
                }
                return answers;
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