using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Server
{
    [ServiceContract(Namespace = "http://queue.name/server/queue-plan")]
    public interface IQueuePlanService : IStandardServerService
    {
        [OperationContract]
        Task<ClientRequestPlan[]> GetOperatorClientRequestPlans();

        [OperationContract]
        Task<Dictionary<Operator, ClientRequestPlan>> GetCurrentClientRequestPlans();

        [OperationContract]
        [WebGet(UriTemplate = "/get-current-client-request-plan", ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequestPlan> GetCurrentClientRequestPlan();

        [OperationContract]
        [WebGet(UriTemplate = "/call-current-client", ResponseFormat = WebMessageFormat.Json)]
        Task CallCurrentClient();

        [OperationContract]
        [WebGet(UriTemplate = "/update-current-client-request?state={state}", ResponseFormat = WebMessageFormat.Json)]
        Task UpdateCurrentClientRequest(ClientRequestState state);

        [OperationContract]
        Task RedirectToOperator(Guid redirectOperatorId);

        [OperationContract]
        Task CallClientByRequestNumber(int number);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task ReturnCurrentClientRequest();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task PostponeCurrentClientRequest(TimeSpan postponeTime);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/edit-current-client-request", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> EditCurrentClientRequest(ClientRequest source);

        [OperationContract]
        Task<QueuePlan> GetQueuePlan(DateTime planDate);

        [OperationContract]
        Task RefreshTodayQueuePlan();
    }
}