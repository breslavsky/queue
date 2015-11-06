using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts.Server
{
    public class ServerService : ClientService<IServerTcpService>
    {
        public ServerService(string endpoint)
            : base(endpoint, ServicesPaths.Server)
        {
        }
    }
}