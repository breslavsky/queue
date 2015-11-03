using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerCorsServiceHost : ServiceHost
    {
        public ServerCorsServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerCorsService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerCorsServiceProvider());
            }
        }
    }
}