using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class LifeSituationHttpServiceHost : ServiceHost
    {
        public LifeSituationHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(LifeSituationHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new LifeSituationHttpServiceProvider());
            }
        }
    }
}