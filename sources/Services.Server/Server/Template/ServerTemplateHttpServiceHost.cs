using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class ServerTemplateHttpServiceHost : ServiceHost
    {
        public ServerTemplateHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(ServerTemplateHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new ServerTemplateHttpServiceProvider());
            }
        }
    }
}