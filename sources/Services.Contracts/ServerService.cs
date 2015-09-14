using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class ServerService<T> : IDisposable
    {
        public DuplexChannelBuilder<T> ChannelBuilder { get; set; }

        public ServerService(string endpoint, string path = "")
        {
            var builder = new UriBuilder(endpoint);
            builder.Path = path;
            ChannelBuilder = new DuplexChannelBuilder<T>(new ServerCallback(),
                Bindings.NetTcpBinding, new EndpointAddress(builder.Uri));
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