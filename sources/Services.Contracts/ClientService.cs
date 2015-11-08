using Junte.WCF;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class ClientService<T> : IDisposable
    {
        public ChannelBuilder<T> ChannelBuilder { get; set; }

        public ClientService(string endpoint, string path)
        {
            var builder = new UriBuilder(endpoint);
            builder.Path = path;
            ChannelBuilder = new ChannelBuilder<T>(Bindings.NetTcpBinding, new EndpointAddress(builder.Uri));
        }

        public ChannelManager<T> CreateChannelManager(Guid SessionId)
        {
            return new ChannelManager<T>(ChannelBuilder, SessionId);
        }

        public ChannelManager<T> CreateChannelManager()
        {
            return new ChannelManager<T>(ChannelBuilder);
        }

        public void Dispose()
        {
            ChannelBuilder.Dispose();
        }
    }
}