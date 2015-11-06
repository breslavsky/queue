using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Hub
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IQualityCallback))]
    public interface IQualityTcpService : IQualityService
    {
        [OperationContract]
        bool IsSubscribed(QualityServiceEventType eventType);

        [OperationContract]
        void Subscribe(QualityServiceEventType eventType, QualityServiceSubscribtionArgs args = null);

        [OperationContract]
        void UnSubscribe(QualityServiceEventType eventType);
    }
}