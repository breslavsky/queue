using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerWorkplaceTcpServiceHost : ServiceHost
    {
        public ServerWorkplaceTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerWorkplaceTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerWorkplaceTcpServiceProvider());
            }
        }
    }
}