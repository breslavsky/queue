using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public interface IClientService<T>
    {
        //TODO: скрыть!
        DuplexChannelBuilder<T> ChannelBuilder { get; set; }

        ChannelManager<T> CreateChannelManager(Guid SessionId);

        ChannelManager<T> CreateChannelManager();

        void Dispose();
    }

    public class ClientService<T> : IClientService<T>
    {
        public DuplexChannelBuilder<T> ChannelBuilder { get; set; }

        private const string Url = "";

        public ClientService(string endpoint)
        {
            ChannelBuilder = new DuplexChannelBuilder<T>(new ServerCallback(),
                Bindings.NetTcpBinding, new EndpointAddress(string.Format("{0}/{1}", endpoint, Url)));
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