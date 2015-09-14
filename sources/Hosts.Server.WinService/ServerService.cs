using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Server;
using Queue.Server.Settings;
using System;
using System.Globalization;
using System.ServiceProcess;
using System.Threading;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Hosts.Server.WinService
{
    public partial class ServerService : ServiceBase
    {
        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static ServerSettings settings;
        private ServerInstance server;

        #endregion fields

        public ServerService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Starting service...");

            try
            {
                container = new UnityContainer();
                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

                configuration = new ConfigurationManager(HostsConsts.ServerApp, SpecialFolder.CommonApplicationData);
                container.RegisterInstance(configuration);

                settings = configuration.GetSection<ServerSettings>(ServerSettings.SectionKey);
                container.RegisterInstance(settings);

                var culture = settings.Language.GetCulture();
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