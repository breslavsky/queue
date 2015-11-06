using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class DisplayHttpServiceHost : ServiceHost
    {
        public DisplayHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(DisplayHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new DisplayHttpServiceProvider());
            }
        }
    }
}