using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerUserTcpServiceHost : ServiceHost
    {
        public ServerUserTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerUserTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerUserTcpServiceProvider());
            }
        }
    }
}