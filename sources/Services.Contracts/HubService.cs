using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class DuplexClientService<T, Callback> : IDisposable where Callback : new()
    {
        public DuplexChannelBuilder<T> ChannelBuilder { get; set; }

        public DuplexClientService(string endpoint, string path)
        {
            var builder = new UriBuilder(endpoint);
            builder.Path = path;
            ChannelBuilder = new DuplexChannelBuilder<T>(new Callback(),
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

    public class HubQualityService : DuplexClientService<IHubQualityTcpService, HubQualityCallback>
    {
        public HubQualityService(string endpoint, string path)
            : base(endpoint, path)
        {
        }
    }
}