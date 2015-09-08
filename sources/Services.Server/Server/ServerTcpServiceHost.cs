using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerTcpServiceHost : ServiceHost
    {
        public ServerTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerTcpServiceProvider());
            }
        }
    }
}