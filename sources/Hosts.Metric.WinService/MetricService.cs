using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Metric;
using System;
using System.ServiceProcess;

namespace Queue.Hosts.Metric.WinService
{
    public partial class MetricService : ServiceBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private MetricInstance metric;

        public MetricService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            logger.Info("Starting service...");

            try
            {
                ConfigurationManager configuration = new ConfigurationManager(HostsConsts.MetricApp);
                MetricSettings settings = configuration.GetSection<MetricSettings>(HostsConsts.MetricSettingsSectionKey);

                metric = new MetricInstance(settings);
                metric.Start();

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
                metric.Stop();
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            logger.Info("Service stopped");
        }
    }
}