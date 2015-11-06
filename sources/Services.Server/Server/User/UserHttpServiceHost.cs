using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class UserHttpServiceHost : ServiceHost
    {
        public UserHttpServiceHost(params Uri[] baseAddresses)
            : base(typeof(UserHttpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new UserHttpServiceProvider());
            }
        }
    }
}