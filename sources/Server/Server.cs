using AutoMapper;
using Junte.Data.NHibernate;
using Junte.WCF.Common;
using log4net;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Caches.SysCache2;
using Queue.Services.Contracts;
using Queue.Services.Server;
using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Description;
using QueuePlan = Queue.Services.DTO.QueuePlan;

namespace Queue.Server
{
    public class ServerInstance
    {
        private const string queuePlanDirectory = "queue-plan";

        private static readonly ILog logger = LogManager.GetLogger(typeof(ServerInstance));

        private ServiceHost tcpServiceHost;
        private ServiceHost httpServiceHost;

        public ServerInstance(ServerSettings settings)
        {
            logger.Info("creating server");

            UnityContainer container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            ISessionProvider sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, settings.Database, (fluently) =>
            {
                fluently.Cache(c => c
                    .ProviderClass<SysCacheProvider>()
                    .UseQueryCache()
                    .UseSecondLevelCache()
                    .UseMinimalPuts());
            });
            container.RegisterInstance<ISessionProvider>(sessionProvider);

            QueueInstance queueInstance = new QueueInstance();
            container.RegisterInstance<IQueueInstance>(queueInstance);

            if (settings.Debug)
            {
                queueInstance.OnTodayQueuePlanBuilded += queueInstance_OnTodayQueuePlanBuilded;
            }

            Mapper.AddProfile(new FullDTOProfile());

            var services = settings.Services;
            var tcpService = services.TcpService;

            Uri uri;

            if (tcpService.Enabled)
            {
                uri = new Uri(string.Format("{0}://{1}:{2}/", Schemes.NET_TCP, tcpService.Host, tcpService.Port));
                logger.InfoFormat("TCP service host uri = ", uri);

                tcpServiceHost = new ServiceHost(typeof(ServerService), uri);
                tcpServiceHost.AddServiceEndpoint(typeof(IServerTcpService), Bindings.NetTcpBinding, string.Empty);
                tcpServiceHost.Description.Behaviors.Add(new ServiceMetadataBehavior());
                tcpServiceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
            }

            var httpService = services.HttpService;

            if (httpService.Enabled)
            {
                uri = new Uri(string.Format("{0}://{1}:{2}/", Schemes.HTTP, httpService.Host, httpService.Port));
                logger.InfoFormat("HTTP service host uri = ", uri);

                httpServiceHost = new ServiceHost(typeof(ServerService), uri);
                httpServiceHost.AddServiceEndpoint(typeof(IServerHttpService), Bindings.BasicHttpBinding, string.Empty);
                httpServiceHost.Description.Behaviors.Add(new ServiceMetadataBehavior()
                {
                    HttpGetEnabled = true
                });
            }
        }

        private void queueInstance_OnTodayQueuePlanBuilded(object sender, QueueInstanceEventArgs e)
        {
            QueuePlan todayQueuePlan = e.QueuePlan;

            try
            {
                if (!Directory.Exists(queuePlanDirectory))
                {
                    Directory.CreateDirectory(queuePlanDirectory);
                }
                string file = Path.Combine(queuePlanDirectory, string.Format("{0:00000}.txt", todayQueuePlan.Version));
                File.WriteAllLines(file, todayQueuePlan.Report);
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        public void Start()
        {
            logger.Info("starting server");

            if (tcpServiceHost != null)
            {
                logger.Info("TCP service host opening");
                tcpServiceHost.Open();
            }

            if (httpServiceHost != null)
            {
                logger.Info("HTTP service host opening");
                httpServiceHost.Open();
            }
        }

        public void Stop()
        {
            logger.Info("stoping server");

            if (tcpServiceHost != null)
            {
                logger.Info("TCP service host closing");
                tcpServiceHost.Close();
            }

            if (httpServiceHost != null)
            {
                logger.Info("HTTP service host closing");
                httpServiceHost.Close();
            }
        }
    }
}