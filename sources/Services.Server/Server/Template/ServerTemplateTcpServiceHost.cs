using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerTemplateTcpServiceHost : ServiceHost
    {
        public ServerTemplateTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerTemplateTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerTemplateTcpServiceProvider());
            }
        }
    }
}