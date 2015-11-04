using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Hosts.Common;
using Queue.Portal;
using Queue.Services.Portal.Settings;
using System;
using System.ServiceProcess;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Hosts.Portal.WinService
{
    public partial class PortalService : ServiceBase
    {
        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private UnityContainer container;
        private ConfigurationManager configuration;
        private PortalSettings settings;
        private LoginSettings loginSettings;
        private PortalServiceSettings portalServiceSettings;
        private PortalInstance portal;

        #endregion fields

        public PortalService()
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

                configuration = new ConfigurationManager(HostsConsts.PortalApp, SpecialFolder.CommonApplicationData);
                container.RegisterInstance(configuration);

                settings = configuration.GetSection<PortalSettings>(PortalSettings.SectionKey);
                container.RegisterInstance(settings);

                loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
                container.RegisterInstance(loginSettings);

                portalServiceSettings = configuration.GetSection<PortalServiceSettings>(PortalServiceSettings.SectionKey);
                container.RegisterInstance(portalServiceSettings);

                portal = new PortalInstance(settings, loginSettings);
                portal.Start();

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
                portal.Stop();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            logger.Info("Service stopped");
        }
    }
}