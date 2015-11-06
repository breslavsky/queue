using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts.Hub
{
    public class QualityService : DuplexClientService<IQualityTcpService, QualityCallback>
    {
        public QualityService(string endpoint)
            : base(endpoint, ServicesPaths.Quality)
        {
        }
    }
}