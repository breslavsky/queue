using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public interface IClientService<T>
    {
        ChannelManager<T> CreateChannelManager(Guid SessionId);
    }

    public class ClientService<T>
    {
        public readonly DuplexChannelBuilder<T> ChannelBuilder;
        private static const string Url = "";

        public ClientService(string endpoint)
        {
            ChannelBuilder = new DuplexChannelBuilder<T>(new ServerCallback(),
                Bindings.NetTcpBinding, new EndpointAddress(string.Format("{0}/{1}", endpoint, Url)));
        }

        public ChannelManager<T> CreateChannelManager(Guid SessionId)
        {
            return new ChannelManager<T>(ChannelBuilder, SessionId);
        }

        public void Dispose()
        {
            ChannelBuilder.Dispose();
        }
    }
}