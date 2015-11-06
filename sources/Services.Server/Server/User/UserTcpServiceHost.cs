using Queue.Services.Contracts;
using System;
using System.ServiceModel;

namespace Queue.Services.Server
{
    public class UserTcpServiceHost : ServiceHost
    {
        public UserTcpServiceHost(params Uri[] baseAddresses)
            : base(typeof(UserTcpService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new UserTcpServiceProvider());
            }
        }
    }
}