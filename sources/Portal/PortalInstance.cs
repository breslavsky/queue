using Junte.WCF.Common;
using Queue.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Services.Portal;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace Queue.Portal
{
    public class PortalInstance
    {
        private Administrator user;
        private bool inited;
        private PortalSettings portalSettings;

        private ServiceHost portalServiceHost;
        private ServiceHost clientServiceHost;
        private ServiceHost operatorServiceHost;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private ServerConnectionSettings serverConnection;

        public PortalInstance(PortalSettings portalSettings, ServerConnectionSettings serverConnection)
        {
            this.portalSettings = portalSettings;
            this.serverConnection = serverConnection;
        }

        public async Task Start()
        {
            if (!inited)
            {
                await ConnectToServer();
            }

            Stop();

            portalServiceHost = CreatePortalServiceHost();
            clientServiceHost = CreateClientServiceHost();
            operatorServiceHost = CreateOperatorServiceHost();

            portalServiceHost.Open();
            clientServiceHost.Open();
            operatorServiceHost.Open();
        }

        private async Task ConnectToServer()
        {
            channelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(),
                                                                           Bindings.NetTcpBinding,
                                                                           new EndpointAddress(serverConnection.Endpoint));

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                user = await channel.Service.UserLogin(serverConnection.User, serverConnection.Password) as Administrator;
            }
        }

        public void Stop()
        {
            StopHost(portalServiceHost);
            StopHost(clientServiceHost);
            StopHost(operatorServiceHost);
        }

        private void StopHost(ServiceHost host)
        {
            if (host != null)
            {
                portalServiceHost.Close();
            }
        }

        private ServiceHost CreatePortalServiceHost()
        {
            Uri uri = new Uri(string.Format("{0}://0.0.0.0:{1}/", Schemes.HTTP, portalSettings.Port));

            PortalServiceHost host = new PortalServiceHost(channelBuilder, user, typeof(PortalService));
            ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(IPortalService), Bindings.WebHttpBinding, uri.ToString());
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            return host;
        }

        private ServiceHost CreateOperatorServiceHost()
        {
            Uri uri = new Uri(string.Format("{0}://0.0.0.0:{1}/operator", Schemes.HTTP, portalSettings.Port));
            PortalOperatorServiceHost host = new PortalOperatorServiceHost(channelBuilder, user, typeof(PortalOperatorService));

            ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(IPortalOperatorService), Bindings.WebHttpBinding, uri);
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            return host;
        }

        private ServiceHost CreateClientServiceHost()
        {
            Uri uri = new Uri(string.Format("{0}://0.0.0.0:{1}/client", Schemes.HTTP, portalSettings.Port));
            ServiceHost host = new PortalClientServiceHost(channelBuilder, user, typeof(PortalClientService));
            ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(IPortalClientService), Bindings.WebHttpBinding, uri);
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            return host;
        }
    }
}