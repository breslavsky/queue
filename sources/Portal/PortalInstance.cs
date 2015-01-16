using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;

namespace Queue.Portal
{
    public class PortalInstance
    {
        private Administrator user;
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;

        public PortalInstance(string endpoint, Administrator user)
        {
            this.channelBuilder = channelBuilder;
            this.user = user;
        }

        public void Start()
        {
        }
    }
}