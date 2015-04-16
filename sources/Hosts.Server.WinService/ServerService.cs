using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Server;
using System;
using System.Globalization;
using System.ServiceProcess;
using System.Threading;

namespace Queue.Hosts.Server.WinService
{
    public partial class ServerService : ServiceBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ServerInstance server;

        public ServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Starting service...");

            try
            {
                ConfigurationManager configuration = new ConfigurationManager(HostsConsts.ServerApp, Environment.SpecialFolder.CommonApplicationData);
                ServerSettings settings = configuration.GetSection<ServerSettings>(HostsConsts.ServerSettingsSectionKey);

                CultureInfo culture = settings.Language.GetCulture();
                Thread.CurrentThread.CurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;

                server = new ServerInstance(settings);
                server.Start();

                logger.Info("Service started");
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw;
            }
        }

        protected override void OnStop()
        {
            logger.Info("Stopping service...");

            try
            {
                server.Stop();
                server.Dispose();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            logger.Info("Service stopped");
        }
    }
}