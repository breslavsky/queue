using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public interface IClientService<T> : IDisposable
    {
        ChannelManager<T> CreateChannelManager(Guid SessionId);

        ChannelManager<T> CreateChannelManager();
    }

    public class ClientService<T> : IClientService<T>
    {
        public DuplexChannelBuilder<T> ChannelBuilder { get; set; }

        public ClientService(string endpoint, string path = "")
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