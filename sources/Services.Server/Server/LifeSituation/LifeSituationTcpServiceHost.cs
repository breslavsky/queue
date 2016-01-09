using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class LifeSituationTcpServiceHost : ServiceHost
    {
        public LifeSituationTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(LifeSituationTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new LifeSituationTcpServiceProvider());
            }
        }
    }
}