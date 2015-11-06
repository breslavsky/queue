using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class WorkplaceHttpServiceHost : ServiceHost
    {
        public WorkplaceHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(WorkplaceHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new WorkplaceHttpServiceProvider());
            }
        }
    }
}