using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class HubDisplayTcpServiceHost : ServiceHost
    {
        public HubDisplayTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(HubDisplayTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new HubDisplayTcpServiceProvider());
            }
        }
    }
}