using Junte.WCF;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Services.Media;
using System;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Threading.Tasks;

namespace Queue.Media
{
    public sealed class MediaInstance
    {
        private bool inited;

        private MediaSettings mediaSettings;
        private LoginSettings serverConnection;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private Administrator user;
        private ServiceHost host;

        public MediaInstance(MediaSettings mediaSettings, LoginSettings serverConnection)
        {
            this.mediaSettings = mediaSettings;
            this.serverConnection = serverConnection;
        }

        public async Task Start()
        {
            if (!inited)
            {
                await ConnectToServer();
            }

            Stop();

            host = CreateHost();
            host.Open();
        }

        private ServiceHost CreateHost()
        {
            ServiceHost host = new MediaServiceHost(channelBuilder, user, mediaSettings.Folder, typeof(MediaService));

            Uri uri = new Uri(string.Format("{0}://0.0.0.0:{1}/", Schemes.Http, mediaSettings.Port));
            ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(typeof(IMediaService), Bindings.WebHttpBinding, uri.ToString());
            serviceEndpoint.Behaviors.Add(new WebHttpBehavior());

            return host;
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

            inited = true;
        }

        public void Stop()
        {
            if (host != null)
            {
                try
                {
                    host.Close();
                    host = null;
                }
                catch { }
            }
        }
    }
}