using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class TemplateTcpServiceHost : ServiceHost
    {
        public TemplateTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(TemplateTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new TemplateTcpServiceProvider());
            }
        }
    }
}