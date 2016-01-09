using AutoMapper;
using Junte.Data.NHibernate;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NHibernate.Caches.SysCache2;
using NLog;
using Queue.Server.Settings;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
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
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.Server));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new ServerTcpServiceHost();
                        host.AddServiceEndpoint(typeof(IServerTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }

                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.Template));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new TemplateTcpServiceHost();
                        host.AddServiceEndpoint(typeof(ITemplateTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }

                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.User));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new UserTcpServiceHost();
                        host.AddServiceEndpoint(typeof(IUserTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }

                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.Workplace));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new WorkplaceTcpServiceHost();
                        host.AddServiceEndpoint(typeof(IWorkplaceTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }

                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.QueuePlan));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new QueuePlanTcpServiceHost();
                        host.AddServiceEndpoint(typeof(IQueuePlanTcpService), Bindings.NetTcpBinding, uri);
                        host.Description.Behaviors.Add(new ServiceMetadataBehavior());

                        hosts.Add(host);
                    }

                    {
                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.NetTcp, tcpService.Host, tcpService.Port, ServicesPaths.LifeSituation));
                        logger.Info("TCP service host uri = {0}", uri);

                        var host = new LifeSituationTcpServiceHost();
                        host.AddServiceEndpoint(typeof(ILifeSituationTcpService), Bindings.NetTcpBinding, uri);
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

                        var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.Server));
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
                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.Template));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var host = new TemplateHttpServiceHost();
                    var endpoint = host.AddServiceEndpoint(typeof(ITemplateHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    endpoint.Behaviors.Add(new EnableCORSBehavior());
                    endpoint.Behaviors.Add(new NoCacheBehavior());
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                    {
                        HttpGetUrl = uri,
                        HttpGetEnabled = true
                    });

                    hosts.Add(host);
                }

                {
                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.User));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var host = new UserHttpServiceHost();
                    var endpoint = host.AddServiceEndpoint(typeof(IUserHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    endpoint.Behaviors.Add(new EnableCORSBehavior());
                    endpoint.Behaviors.Add(new NoCacheBehavior());
                    endpoint.Behaviors.Add(new ExtendedWebHttpBehavior());
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                    {
                        HttpGetUrl = uri,
                        HttpGetEnabled = true
                    });

                    hosts.Add(host);
                }

                {
                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.Workplace));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var host = new WorkplaceHttpServiceHost();
                    var endpoint = host.AddServiceEndpoint(typeof(IWorkplaceHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    endpoint.Behaviors.Add(new EnableCORSBehavior());
                    endpoint.Behaviors.Add(new NoCacheBehavior());
                    endpoint.Behaviors.Add(new ExtendedWebHttpBehavior());
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                    {
                        HttpGetUrl = uri,
                        HttpGetEnabled = true
                    });

                    hosts.Add(host);
                }

                {
                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.QueuePlan));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var host = new QueuePlanHttpServiceHost();
                    var endpoint = host.AddServiceEndpoint(typeof(IQueuePlanHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    endpoint.Behaviors.Add(new EnableCORSBehavior());
                    endpoint.Behaviors.Add(new NoCacheBehavior());
                    endpoint.Behaviors.Add(new ExtendedWebHttpBehavior());
                    host.Description.Behaviors.Add(new ServiceMetadataBehavior()
                    {
                        HttpGetUrl = uri,
                        HttpGetEnabled = true
                    });

                    hosts.Add(host);
                }

                {
                    var uri = new Uri(string.Format("{0}://{1}:{2}/{3}", Schemes.Http, httpService.Host, httpService.Port, ServicesPaths.LifeSituation));
                    logger.Info("HTTP service host uri = {0}", uri);

                    var host = new LifeSituationHttpServiceHost();
                    var endpoint = host.AddServiceEndpoint(typeof(ILifeSituationHttpService), Bindings.WebHttpBinding, uri);
                    endpoint.Behaviors.Add(new WebHttpBehavior());
                    endpoint.Behaviors.Add(new EnableCORSBehavior());
                    endpoint.Behaviors.Add(new NoCacheBehavior());
                    endpoint.Behaviors.Add(new ExtendedWebHttpBehavior());
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