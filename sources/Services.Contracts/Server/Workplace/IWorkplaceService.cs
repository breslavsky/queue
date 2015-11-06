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
    [ServiceContract(Namespace = "http://queue.name/server/workplace")]
    public interface IWorkplaceService : IStandardServerService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/get-workplaces-links", ResponseFormat = WebMessageFormat.Json)]
        Task<IdentifiedEntityLink[]> GetWorkplacesLinks();

        [OperationContract]
        [WebGet(UriTemplate = "/get-workplaces", ResponseFormat = WebMessageFormat.Json)]
        Task<Workplace[]> GetWorkplaces();

        [OperationContract]
        [WebGet(UriTemplate = "/get-workplace?workplaceId={workplaceId}", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Workplace> GetWorkplace(Guid workplaceId);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/edit-workplace", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Workplace> EditWorkplace(Workplace workplace);

        [OperationContract]
        [WebGet(UriTemplate = "/delete?workplaceId={workplaceId}", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task DeleteWorkplace(Guid workplaceId);

        [OperationContract]
        [WebGet(UriTemplate = "/get-workplace-operators?workplaceId={workplaceId}", ResponseFormat = WebMessageFormat.Json)]
        Task<Operator[]> GetWorkplaceOperators(Guid workplaceId);
    }
}