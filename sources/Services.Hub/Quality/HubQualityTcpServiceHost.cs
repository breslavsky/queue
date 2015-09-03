using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class HubQualityTcpServiceHost : ServiceHost
    {
        public HubQualityTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(HubQualityTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new HubQualityTcpServiceProvider());
            }
        }
    }
}