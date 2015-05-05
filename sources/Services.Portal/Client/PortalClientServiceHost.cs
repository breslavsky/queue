using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;

namespace Queue.Services.Portal
{
    public sealed class PortalClientServiceHost : ServiceHost
    {
        public PortalClientServiceHost(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new PortalClientServiceProvider(channelBuilder, currentUser));
            }
        }
    }
}