using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IHubQualityCallback))]
    public interface IHubQualityTcpService : IHubQualityService
    {
        [OperationContract]
        bool IsSubscribed(HubQualityServiceEventType eventType);

        [OperationContract]
        void Subscribe(HubQualityServiceEventType eventType, HubQualityServiceSubscribtionArgs args = null);

        [OperationContract]
        void UnSubscribe(HubQualityServiceEventType eventType);
    }
}