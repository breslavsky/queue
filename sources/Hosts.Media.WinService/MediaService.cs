using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Hosts.Common;
using Queue.Media;
using Queue.Services.Media.Settings;
using System;
using System.ServiceProcess;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Hosts.Media.WinService
{
    public partial class MediaService : ServiceBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static MediaSettings settings;
        private static MediaServiceSettings mediaServiceSettings;
        private MediaInstance media;

        public MediaService()
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

                configuration = new ConfigurationManager(HostMetadata.ServerApp, SpecialFolder.CommonApplicationData);
                container.RegisterInstance(configuration);

                settings = configuration.GetSection<MediaSettings>(MediaSettings.SectionKey);
                container.RegisterInstance(settings);

                mediaServiceSettings = configuration.GetSection<MediaServiceSettings>(MediaServiceSettings.SectionKey);
                container.RegisterInstance(mediaServiceSettings);

                media = new MediaInstance(settings);
                media.Start();

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
                media.Stop();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            logger.Info("Service stopped");
        }
    }
}