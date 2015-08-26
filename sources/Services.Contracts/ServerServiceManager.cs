using Junte.WCF;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    public interface IServerServiceManager
    {
        Queue.Services.Contracts.ServerServiceManager.ServerService Server { get; set; }

        void Dispose();
    }

    public class ServerServiceManager : IServerServiceManager
    {
        public class ServerService
        {
            public DuplexChannelBuilder<IServerTcpService> ChannelBuilder;
            private const string url = "";
            public Guid SessionId;

            public ServerService(string scheme, string host, int port)
            {
                string endpoint = string.Format("{0}://{1}:{2}/{3}", scheme, host, port, url);

                ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(),
                    Bindings.NetTcpBinding, new EndpointAddress(endpoint));
            }

            public ChannelManager<IServerTcpService> CreateChannelManager()
            {
                return new ChannelManager<IServerTcpService>(ChannelBuilder, SessionId);
            }

            public void Dispose()
            {
            }
        }

        public ServerService Server { get; set; }

        public Guid SessionId
        {
            set
            {
                Server.SessionId = value;
            }
        }

        public ServerServiceManager(string scheme, string host, int port)
        {
            Server = new ServerService(scheme, host, port);
        }

        public void Dispose()
        {
            if (Server != null)
            {
                Server.Dispose();
            }
        }
    }
}