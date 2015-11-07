using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Server.Settings;
using System;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class ServerTemplateService : DependencyService, IServerTemplateService
    {
        #region dependency

        [Dependency]
        public TemplateServiceSettings Settings { get; set; }

        #endregion dependency

        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IContextChannel channel;
        private readonly string templatesFolder;

        #endregion fields

        public ServerTemplateService()
            : base()
        {
            logger.Debug("Создан новый экземпляр службы");

            channel = OperationContext.Current.Channel;
            channel.Faulted += channel_Faulted;
            channel.Closing += channel_Closing;

            var currentDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

            templatesFolder = !string.IsNullOrWhiteSpace(Settings.Folder)
                ? Settings.Folder
                : Path.Combine(currentDirectory, "templates");
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
            return File.Open(Path.Combine(templatesFolder, app, theme, template), FileMode.Open);
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