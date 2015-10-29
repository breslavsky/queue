using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;

namespace Queue.Services.Portal
{
    public class PortalServiceHost : ServiceHost
    {
        public PortalServiceHost(params Uri[] baseAddresses)
            : base(typeof(PortalService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new PortalServiceProvider());
            }
        }
    }
}