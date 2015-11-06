using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class QueuePlanTcpServiceHost : ServiceHost
    {
        public QueuePlanTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(QueuePlanTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new QueuePlanTcpServiceProvider());
            }
        }
    }
}