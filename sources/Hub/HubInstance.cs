﻿using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Hub.Settings;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Hub;
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
        #region dependency

        [Dependency]
        public IUnityContainer Container { get; set; }

        #endregion dependency

        #region fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IList<ServiceHost> hosts = new List<ServiceHost>();
        private readonly IList<IQualityDriver> qualityDrivers = new List<IQualityDriver>();
        private readonly IList<IDisplayDriver> displayDrivers = new List<IDisplayDriver>();
        private readonly HubSettings settings;
        private bool disposed;

        #endregion fields

        public HubInstance(HubSettings settings)
        {
            this.settings = settings;
            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this);

            LoadDrivers();
            CreateServices();
        }

        private void LoadDrivers()
        {
            foreach (DriverElementConfig d in settings.Drivers.Display)
            {
                var config = d.Config as DriverConfig;
                var type = Assembly.Load(config.Assembly).GetType(config.Type);

                var driver = Activator.CreateInstance(type, config) as IDisplayDriver;
                if (driver == null)
                {
                    throw new ApplicationException(string.Format("Error load display driver {0} from {1}", config.Type, config.Assembly));
                }

                displayDrivers.Add(driver);
            }

            Container.RegisterInstance(displayDrivers.ToArray());

            foreach (DriverElementConfig d in settings.Drivers.Quality)
            {
                var config = d.Config as DriverConfig;
                var type = Assembly.Load(config.Assembly).GetType(config.Type);

                var driver = Activator.CreateInstance(type, config) as IQualityDriver;
                if (driver == null)
                {
                    throw new ApplicationException(string.Format("Error load quality driver {0} from {1}", config.Type, config.Assembly));
                }

                qualityDrivers.Add(driver);
            }

            Container.RegisterInstance(qualityDrivers.ToArray());
        }

        private void CreateServices()
        {
            var services = settings.Services;
            var tcpService = services.TcpService;

            if (tcpService.Enabled)
            {
                {
                    //quality
                    var host = new QualityTcpServiceHost();

                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.Quality));
                    logger.Info("TCP service host uri = {0}", uri);

                    host.AddServiceEndpoint(typeof(IQualityTcpService), Bindings.NetTcpBinding, uri);
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior());
                    hosts.Add(host);
                }

                {
                    //display
                    var host = new DisplayTcpServiceHost();

                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.Display));
                    logger.Info("TCP service host uri = {0}", uri);

                    host.AddServiceEndpoint(typeof(IDisplayTcpService), Bindings.NetTcpBinding, uri);
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                    hosts.Add(host);
                }
            }

            var httpService = services.HttpService;

            if (httpService.Enabled)
            {
                {
                    //quality
                    var host = new QualityHttpServiceHost();

                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.Quality));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var endpoint = host.AddServiceEndpoint(typeof(IQualityHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                    {
                        HttpGetUrl = uri,
                        HttpGetEnabled = true
                    });

                    hosts.Add(host);
                }

                {
                    //display
                    var host = new DisplayHttpServiceHost();

                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.Display));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var endpoint = host.AddServiceEndpoint(typeof(IDisplayHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                    {
                        HttpGetUrl = uri,
                        HttpGetEnabled = true
                    });

                    hosts.Add(host);
                }
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
                hosts.Clear();
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