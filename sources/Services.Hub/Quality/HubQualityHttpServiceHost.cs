using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class HubQualityHttpServiceHost : ServiceHost
    {
        public HubQualityHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(HubQualityHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new HubQualityHttpServiceProvider());
            }
        }
    }
}