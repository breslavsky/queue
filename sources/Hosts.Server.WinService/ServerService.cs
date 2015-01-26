using log4net;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Server;
using System;
using System.ServiceProcess;

namespace Queue.Hosts.Server.WinService
{
    public partial class ServerService : ServiceBase
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(ServerService));

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
                ConfigurationManager configuration = new ConfigurationManager(AppNames.ServerApp);
                ServerSettings settings = configuration.GetSection<ServerSettings>("server");

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