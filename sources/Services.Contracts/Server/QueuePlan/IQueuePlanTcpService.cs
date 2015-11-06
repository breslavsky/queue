using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Server
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IQueuePlanCallback))]
    public interface IQueuePlanTcpService : IQueuePlanService
    {
        [OperationContract]
        bool IsSubscribed(QueuePlanEventType eventType);

        [OperationContract]
        void Subscribe(QueuePlanEventType eventType, QueuePlanSubscribtionArgs args = null);

        [OperationContract]
        void UnSubscribe(QueuePlanEventType eventType);
    }
}