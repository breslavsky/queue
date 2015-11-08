using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Portal;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Services.Portal;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace Queue.Portal
{
    public sealed class PortalInstance
    {
        #region dependency

        [Dependency]
        public IUnityContainer Container { get; set; }

        #endregion dependency

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private PortalSettings portalSettings;
        private LoginSettings loginSettings;
        private readonly IList<ServiceHost> hosts = new List<ServiceHost>();

        public PortalInstance(PortalSettings portalSettings, LoginSettings loginSettings)
        {
            this.portalSettings = portalSettings;
            this.loginSettings = loginSettings;

            ServiceLocator.Current.GetInstance<UnityContainer>()
                .BuildUp(this);

            Container.RegisterInstance(portalSettings);

            CreateServices();
        }

        private void ConnectToServer()
        {
            var serverUserService = new UserService(loginSettings.Endpoint);

            Administrator currentUser;

            using (var channelManager = serverUserService.CreateChannelManager())
            using (var channel = channelManager.CreateChannel())
            {
                currentUser = channel.Service.UserLogin(loginSettings.User, loginSettings.Password).Result as Administrator;
                Container.RegisterInstance(currentUser);
            }

            Container.RegisterInstance(serverUserService);
            Container.RegisterType<ChannelManager<IServerTcpService>>
                (new InjectionFactory(c => serverUserService.CreateChannelManager(currentUser.SessionId)));

            var serverService = new ServerService(loginSettings.Endpoint);
            Container.RegisterInstance(serverService);
            Container.RegisterType<ChannelManager<IServerTcpService>>
                (new InjectionFactory(c => serverService.CreateChannelManager(currentUser.SessionId)));

            var templateManager = new TemplateManager(Templates.Apps.Common, portalSettings.Theme);
            Container.RegisterInstance<ITemplateManager>(templateManager);
        }

        private void CreateServices()
        {
            {
                var host = new PortalServiceHost();

                var uri = new Uri(string.Format("{0}://0.0.0.0:{1}/", Schemes.Http, portalSettings.Port));
                var endpoint = host.AddServiceEndpoint(typeof(IPortalService), Bindings.WebHttpBinding, uri.ToString());
                endpoint.Behaviors.Add(new WebHttpBehavior());
                endpoint.Behaviors.Add(new EnableCORSBehavior());

                hosts.Add(host);
            }
            {
                var host = new PortalOperatorServiceHost();

                var uri = new Uri(string.Format("{0}://0.0.0.0:{1}/operator", Schemes.Http, portalSettings.Port));
                var endpoint = host.AddServiceEndpoint(typeof(IPortalOperatorService), Bindings.WebHttpBinding, uri);
                endpoint.Behaviors.Add(new WebHttpBehavior());
                endpoint.Behaviors.Add(new EnableCORSBehavior());

                hosts.Add(host);
            }

            {
                var host = new PortalClientServiceHost();

                var uri = new Uri(string.Format("{0}://0.0.0.0:{1}/client", Schemes.Http, portalSettings.Port));
                var endpoint = host.AddServiceEndpoint(typeof(IPortalClientService), Bindings.WebHttpBinding, uri);
                endpoint.Behaviors.Add(new WebHttpBehavior());
                endpoint.Behaviors.Add(new EnableCORSBehavior());

                hosts.Add(host);
            }
        }

        public void Start()
        {
            logger.Info("Starting");

            ConnectToServer();

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
    }
}