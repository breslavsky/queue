using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Contracts;
using Queue.Services.Hub;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Queue.Server
{
    public sealed class HubInstance : IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IList<ServiceHost> hosts = new List<ServiceHost>();
        private bool disposed;

        public HubInstance(HubSettings settings)
        {
            var container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            var services = settings.Services;
            var tcpService = services.TcpService;

            Uri uri;

            if (tcpService.Enabled)
            {
                //quality
                uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NET_TCP, tcpService.Host, tcpService.Port, HubServicesPaths.Quality));
                logger.Info("TCP service host uri = ", uri);

                var host = new ServiceHost(typeof(HubQualityService), uri);
                host.AddServiceEndpoint(typeof(IHubQualityService), Bindings.NetTcpBinding, string.Empty);
                host.Description.Behaviors.Add(new ServiceMetadataBehavior());
                //https://msdn.microsoft.com/en-us/library/dn178463(v=pandp.30).aspx#sec29
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
                hosts.Add(host);
            }

            var httpService = services.HttpService;

            if (httpService.Enabled)
            {
                //quality
                uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.HTTP, httpService.Host, httpService.Port, HubServicesPaths.Quality));
                logger.Info("HTTP service host uri = ", uri);

                var host = new ServiceHost(typeof(HubQualityService), uri);
                var endpoint = host.AddServiceEndpoint(typeof(IHubQualityService), Bindings.WebHttpBinding, string.Empty);
                //endpoint.Behaviors.Add(new WebHttpBehavior());
                host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                {
                    HttpGetEnabled = true
                });

                hosts.Add(host);
            }
        }

        public void Start()
        {
            logger.Info("Starting");

            foreach (var h in hosts)
            {
                h.Open();
            }
        }

        public void Stop()
        {
            logger.Info("Stoping");

            foreach (var h in hosts)
            {
                h.Close();
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
            }

            disposed = true;
        }

        ~HubInstance()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}