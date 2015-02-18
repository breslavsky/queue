using AutoMapper;
using Junte.Data.NHibernate;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Caches.SysCache2;
using NLog;
using Queue.Services.Contracts;
using Queue.Services.Server;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Queue.Server
{
    public sealed class ServerInstance : IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private ServiceHost tcpServiceHost;
        private ServiceHost httpServiceHost;
        private bool disposed;

        public ServerInstance(ServerSettings settings)
        {
            logger.Info("Creating.... [server: {0}; database: {1}]", settings.Database.Server, settings.Database.Name);

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

            Mapper.AddProfile(new FullDTOProfile());

            ServicesConfig services = settings.Services;
            TcpServiceConfig tcpService = services.TcpService;

            Uri uri;

            if (tcpService.Enabled)
            {
                uri = new Uri(string.Format("{0}://{1}:{2}/", Schemes.NET_TCP, tcpService.Host, tcpService.Port));
                logger.Info("TCP service host uri = ", uri);

                tcpServiceHost = new ServiceHost(typeof(ServerService), uri);
                tcpServiceHost.AddServiceEndpoint(typeof(IServerTcpService), Bindings.NetTcpBinding, string.Empty);
                tcpServiceHost.Description.Behaviors.Add(new ServiceMetadataBehavior());
                tcpServiceHost.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
            }

            var httpService = services.HttpService;

            if (httpService.Enabled)
            {
                uri = new Uri(string.Format("{0}://{1}:{2}/", Schemes.HTTP, httpService.Host, httpService.Port));
                logger.Info("HTTP service host uri = ", uri);

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

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (tcpServiceHost != null)
                {
                    tcpServiceHost.Close();
                    tcpServiceHost = null;
                }

                if (httpServiceHost != null)
                {
                    httpServiceHost.Close();
                    httpServiceHost = null;
                }
            }

            disposed = true;
        }

        ~ServerInstance()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}