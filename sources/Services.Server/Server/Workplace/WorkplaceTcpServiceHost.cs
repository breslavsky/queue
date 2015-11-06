using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class WorkplaceTcpServiceHost : ServiceHost
    {
        public WorkplaceTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(WorkplaceTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new WorkplaceTcpServiceProvider());
            }
        }
    }
}