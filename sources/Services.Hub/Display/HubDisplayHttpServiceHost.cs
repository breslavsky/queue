using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class HubDisplayHttpServiceHost : ServiceHost
    {
        public HubDisplayHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(HubDisplayHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new HubDisplayHttpServiceProvider());
            }
        }
    }
}