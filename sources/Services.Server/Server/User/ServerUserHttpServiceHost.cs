using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerUserHttpServiceHost : ServiceHost
    {
        public ServerUserHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerUserHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerUserHttpServiceProvider());
            }
        }
    }
}