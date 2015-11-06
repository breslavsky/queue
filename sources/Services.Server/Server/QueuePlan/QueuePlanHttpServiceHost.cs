using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class QueuePlanHttpServiceHost : ServiceHost
    {
        public QueuePlanHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(QueuePlanHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new QueuePlanHttpServiceProvider());
            }
        }
    }
}