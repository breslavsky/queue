using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class ServerWorkplaceService : ClientService<IServerWorkplaceTcpService>
    {
        public ServerWorkplaceService(string endpoint)
            : base(endpoint, ServerServicesPaths.Workplace)
        {
        }
    }
}