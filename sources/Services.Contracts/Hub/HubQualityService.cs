using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts
{
    public class HubQualityService : DuplexClientService<IHubQualityTcpService, HubQualityCallback>
    {
        public HubQualityService(string endpoint, string path)
            : base(endpoint, path)
        {
        }
    }
}