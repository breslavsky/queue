using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class ServerUserService : ClientService<IServerUserTcpService>
    {
        public ServerUserService(string endpoint)
            : base(endpoint, ServerServicesPaths.User)
        {
        }
    }
}