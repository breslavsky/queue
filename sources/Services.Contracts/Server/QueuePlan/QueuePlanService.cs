using Junte.WCF;
using Queue.Services.Common;
using System;
using System.ServiceModel;

namespace Queue.Services.Contracts.Server
{
    public class QueuePlanService : DuplexClientService<IQueuePlanTcpService, QueuePlanCallback>
    {
        public QueuePlanService(string endpoint)
            : base(endpoint, ServicesPaths.QueuePlan)
        {
        }
    }
}