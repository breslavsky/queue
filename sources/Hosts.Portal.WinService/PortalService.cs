using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Portal;
using System;
using System.ServiceProcess;

namespace Hosts.Portal.WinService
{
    public partial class PortalService : ServiceBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private PortalInstance portal;

        public PortalService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            logger.Info("Starting service...");

            try
            {
                ConfigurationManager configuration = new ConfigurationManager(HostsConsts.PortalApp, Environment.SpecialFolder.CommonApplicationData);
                PortalSettings portalSettings = configuration.GetSection<PortalSettings>(HostsConsts.PortalSettingsSectionKey);
                LoginSettings connectionSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);

                portal = new PortalInstance(portalSettings, connectionSettings);
                await portal.Start();

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