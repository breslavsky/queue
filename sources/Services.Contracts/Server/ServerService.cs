using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class ServerService : DuplexClientService<IServerTcpService, ServerCallback>
    {
        public ServerService(string endpoint)
            : base(endpoint, ServerServicesPaths.Server)
        {
        }
    }
}