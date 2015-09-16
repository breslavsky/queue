using Junte.WCF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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

        public DuplexChannelManager<T> CreateChannelManager(Guid SessionId)
        {
            return new DuplexChannelManager<T>(ChannelBuilder, SessionId);
        }

        public DuplexChannelManager<T> CreateChannelManager()
        {
            return new DuplexChannelManager<T>(ChannelBuilder);
        }

        public void Dispose()
        {
            ChannelBuilder.Dispose();
        }
    }
}