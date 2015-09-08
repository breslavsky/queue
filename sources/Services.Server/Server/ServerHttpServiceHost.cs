using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerHttpServiceHost : ServiceHost
    {
        public ServerHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerHttpServiceProvider());
            }
        }
    }
}