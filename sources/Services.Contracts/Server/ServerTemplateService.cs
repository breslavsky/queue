using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class ServerTemplateService : ClientService<IServerTcpService>
    {
        public ServerTemplateService(string endpoint, string path)
            : base(endpoint, path)
        {
        }
    }
}