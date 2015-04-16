using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Media;
using System;
using System.ServiceProcess;

namespace Queue.Hosts.Media.WinService
{
    public partial class MediaService : ServiceBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private MediaInstance media;

        public MediaService()
        {
            InitializeComponent();
        }

        protected override async void OnStart(string[] args)
        {
            logger.Info("Starting service...");

            try
            {
                ConfigurationManager configuration = new ConfigurationManager(HostsConsts.MediaApp, Environment.SpecialFolder.CommonApplicationData);
                MediaSettings mediaSettings = configuration.GetSection<MediaSettings>(HostsConsts.MediaSettingsSectionKey);
                LoginSettings connectionSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);

                media = new MediaInstance(mediaSettings, connectionSettings);
                await media.Start();

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