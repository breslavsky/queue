using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerWorkplaceHttpServiceHost : ServiceHost
    {
        public ServerWorkplaceHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerWorkplaceHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerWorkplaceHttpServiceProvider());
            }
        }
    }
}