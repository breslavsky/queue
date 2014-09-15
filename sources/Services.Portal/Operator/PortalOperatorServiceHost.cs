using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;

namespace Queue.Services.Portal
{
    public class PortalOperatorServiceHost : ServiceHost
    {
        public PortalOperatorServiceHost(DuplexChannelBuilder<IServerService> channelBuilder, Administrator currentUser, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new PortalOperatorServiceProvider(channelBuilder, currentUser));
            }
        }
    }
}