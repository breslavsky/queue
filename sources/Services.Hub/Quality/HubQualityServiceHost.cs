using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class HubQualityServiceHost : ServiceHost
    {
        public HubQualityServiceHost(params Uri[] baseAddresses)
            : base(typeof(HubQualityService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new HubQualityServiceProvider());
            }
        }
    }
}