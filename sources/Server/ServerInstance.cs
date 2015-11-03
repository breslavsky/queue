using AutoMapper;
using Junte.Data.NHibernate;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Caches.SysCache2;
using NLog;
using Queue.Server.Settings;
using Queue.Services.Contracts;
using Queue.Services.Server;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Queue.Server
{
    public sealed class ServerInstance : IDisposable
    {
        #region dependency

        [Dependency]
        public IUnityContainer Container { get; set; }

        #endregion dependency

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IList<ServiceHost> hosts = new List<ServiceHost>();
        private readonly ServerSettings settings;
        private bool disposed;

        public ServerInstance(ServerSettings settings)
        {
            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this);

            this.settings = settings;

            DatabaseConnect();
            CreateQueueInstance();
            RegisterDTOMapping();
            CreateServices();
        }

        private void DatabaseConnect()
        {
            Container.RegisterInstance(new SessionProvider(new string[] { "Queue.Model" },
                settings.Database, (fluently) =>
                {
                    fluently.Cache(c => c
                        .ProviderClass<SysCacheProvider>()
                        .UseQueryCache()
                        .UseSecondLevelCache()
                        .UseMinimalPuts());
                }));
        }

        private void CreateQueueInstance()
        {
            Container.RegisterInstance(new QueueInstance());
        }

        private void RegisterDTOMapping()
        {
            Mapper.AddProfile(new FullDTOProfile());
        }

        private void CreateServices()
        {
            var services = settings.Services;
            var tcpService = services.TcpService;

            if (tcpService.Enabled)
            {
                {
                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServerServicesPaths.Server));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new ServerTcpServiceHost();
                        host.AddServiceEndpoint(typeof(IServerTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }

                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServerServicesPaths.Template));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new ServerTemplateTcpServiceHost();
                        host.AddServiceEndpoint(typeof(IServerTemplateTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }
                }
            }

            var httpService = services.HttpService;

            if (httpService.Enabled)
            {
                {
                    {
                        var host = new ServerHttpServiceHost();

                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServerServicesPaths.Server));
                        logger.Info("HTTP service host uri = {0}", uri);

                        var endpoint = host.AddServiceEndpoint(typeof(IServerHttpService), Bindings.BasicHttpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                        {
                            HttpGetUrl = uri,
                            HttpGetEnabled = true
                        });

                        hosts.Add(host);
                    }
                }

                {
                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServerServicesPaths.Template));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var host = new ServerTemplateHttpServiceHost();
                    var endpoint = host.AddServiceEndpoint(typeof(IServerTemplateHttpService), Bindings.WebHttpBinding, uri);
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

        ~ServerInstance()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}