using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;

namespace Queue.Services.Portal
{
    public class PortalOperatorServiceHost : ServiceHost
    {
        public PortalOperatorServiceHost(params Uri[] baseAddresses)
            : base(typeof(PortalOperatorService), baseAddresses)
        {
            foreach (var d in ImplementedContracts.Values)
            {
                d.Behaviors.Add(new PortalOperatorServiceProvider());
            }
        }
    }
}