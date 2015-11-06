using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Hub
{
    public class DisplayTcpServiceHost : ServiceHost
    {
        public DisplayTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(DisplayTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new DisplayTcpServiceProvider());
            }
        }
    }
}