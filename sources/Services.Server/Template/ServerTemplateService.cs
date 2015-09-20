using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class ServerTemplateService : DependencyService, IServerTemplateService
    {
        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IContextChannel channel;

        #endregion fields

        public ServerTemplateService()
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

        protected Stream ReadTemplate(string app, string theme, string template)
        {
            string contentPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "templates");
            string file = Path.Combine(contentPath, app, theme, template);
            return File.Open(file, FileMode.Open);
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