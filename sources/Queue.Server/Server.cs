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
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Queue
{
    public class Server
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Server));

        private UnityContainer container;

        private ISessionProvider sessionProvider;

        private ServiceHost tcpServiceHost;
        private ServiceHost httpServiceHost;

        public Server(ServerSettings settings)
        {
            logger.Info("Creating");

            container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() =>
                new UnityServiceLocator(container));

            var queueInstance = new QueueInstance();
            container.RegisterInstance<IQueueInstance>(queueInstance);

            sessionProvider = new SessionProvider(new string[] { "Queue.Model" }, settings.Database, (fluently) =>
            {
                fluently.Cache(c => c
                    .ProviderClass<SysCacheProvider>()
                    .UseQueryCache()
                    .UseSecondLevelCache()
                    .UseMinimalPuts());
            });
            container.RegisterInstance<ISessionProvider>(sessionProvider);

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

        public void Start()
        {
            logger.Info("Starting");

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