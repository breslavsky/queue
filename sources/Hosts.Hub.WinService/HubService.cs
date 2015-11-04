using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Hub;
using Queue.Hub.Settings;
using System;
using System.Globalization;
using System.ServiceProcess;
using System.Threading;

namespace Queue.Hosts.Hub.WinService
{
    public partial class HubService : ServiceBase
    {
        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static HubSettings settings;
        private HubInstance hub;

        #endregion fields

        public HubService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Starting service...");

            try
            {
                container = new UnityContainer();
                container.RegisterInstance(container);
                ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

                configuration = new ConfigurationManager(HostsConsts.HubApp, Environment.SpecialFolder.CommonApplicationData);
                container.RegisterInstance(configuration);

                settings = configuration.GetSection<HubSettings>(HubSettings.SectionKey);
                container.RegisterInstance(settings);

                var culture = settings.Language.GetCulture();
                Thread.CurrentThread.CurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;

                hub = new HubInstance(settings);
                hub.Start();

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
                hub.Stop();
                hub.Dispose();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            logger.Info("Service stopped");
        }
    }
}