using log4net;
using Queue.Server;
using System;
using System.Configuration;
using System.ServiceProcess;

namespace Queue.Hosts.Server.WinService
{
    public partial class ServerService : ServiceBase
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ServerService));

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
                ServerSettings settings = ConfigurationManager.GetSection("server") as ServerSettings;

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
            }
            catch (Exception e)
            {
                logger.Error(e);
            }

            logger.Info("Service stopped");
        }
    }
}