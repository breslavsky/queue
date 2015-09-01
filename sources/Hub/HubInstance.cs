using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Contracts;
using Queue.Services.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Queue.Hub
{
    public sealed class HubInstance : IDisposable
    {
        #region fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private IList<ServiceHost> hosts = new List<ServiceHost>();
        private IList<IHubQualityDriver> qualityDrivers = new List<IHubQualityDriver>();
        private HubSettings settings;
        private bool disposed;

        #endregion fields

        public HubInstance(HubSettings settings)
        {
            this.settings = settings;

            LoadDrivers();
            LoadServices();
        }

        private void LoadDrivers()
        {
            foreach (var d in settings.Drivers.Quality)
            {
                var assembly = Assembly.LoadFrom(d.Assembly);
                var type = assembly.GetType(d.Type);

                var driver = Activator.CreateInstance(type, d) as IHubQualityDriver;
                if (driver == null)
                {
                    throw new ApplicationException(string.Format("Error load quality driver {0} from {1}", d.Type, d.Assembly));
                }
            }

            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            container.RegisterInstance<IHubQualityDriver[]>(qualityDrivers.ToArray());
        }

        private void LoadServices()
        {
            var services = settings.Services;
            var tcpService = services.TcpService;

            Uri uri;

            if (tcpService.Enabled)
            {
                //quality
                var host = new HubQualityServiceHost();

                uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NET_TCP, tcpService.Host, tcpService.Port, HubServicesPaths.Quality));
                logger.Info("TCP service host uri = ", uri);

                host.AddServiceEndpoint(typeof(IHubQualityTcpService), Bindings.NetTcpBinding, string.Empty);
                host.Description.Behaviors.Add(new ServiceMetadataBehavior());
                host.AddServiceEndpoint(typeof(IMetadataExchange), MetadataExchangeBindings.CreateMexTcpBinding(), "mex");
                hosts.Add(host);
            }

            var httpService = services.HttpService;

            if (httpService.Enabled)
            {
                //quality
                var host = new HubQualityServiceHost();

                uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.HTTP, httpService.Host, httpService.Port, HubServicesPaths.Quality));
                logger.Info("HTTP service host uri = ", uri);

                var endpoint = host.AddServiceEndpoint(typeof(IHubQualityHttpService), Bindings.BasicHttpBinding, string.Empty);
                endpoint.Behaviors.Add(new WebHttpBehavior());
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