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
    [ServiceContract(Namespace = "http://queue.name/server/life-situation")]
    public interface ILifeSituationService : IStandardServerService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/get-root-groups", ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituationGroup[]> GetRootGroups();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/get-groups?parentGroupId={parentGroupId}", ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituationGroup[]> GetGroups(Guid parentGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/get-group?groupId={groupId}", ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituationGroup> GetGroup(Guid groupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebInvoke(Method = "POST", UriTemplate = "/edit-group", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituationGroup> EditGroup(LifeSituationGroup group);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/move-group?sourceGroupId={sourceGroupId}&targetGroupId={targetGroupId}", ResponseFormat = WebMessageFormat.Json)]
        Task MoveGroup(Guid sourceGroupId, Guid targetGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/move-group-to-root?sourceGroupId={sourceGroupId}", ResponseFormat = WebMessageFormat.Json)]
        Task MoveGroupToRoot(Guid sourceGroupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/group-up?groupId={groupId}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> GroupUp(Guid groupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/group-down?groupId={groupId}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> GroupDown(Guid groupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/delete-group?groupId={groupId}", ResponseFormat = WebMessageFormat.Json)]
        Task DeleteGroup(Guid groupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/get-root-life-situations", ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituation[]> GetRootLifeSituations();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/get-life-situations?groupId={groupId}", ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituation[]> GetLifeSituations(Guid groupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/get-life-situation?lifeSituationId={lifeSituationId}", ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituation> GetLifeSituation(Guid lifeSituationId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebInvoke(Method = "POST", UriTemplate = "/edit-life-situation", RequestFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.WrappedRequest, ResponseFormat = WebMessageFormat.Json)]
        Task<LifeSituation> EditLifeSituation(LifeSituation lifeSituation);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/move-life-situation?lifeSituationId={lifeSituationId}&groupId={groupId}", ResponseFormat = WebMessageFormat.Json)]
        Task MoveLifeSituation(Guid lifeSituationId, Guid groupId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/delete-life-situation?lifeSituationId={lifeSituationId}", ResponseFormat = WebMessageFormat.Json)]
        Task DeleteLifeSituation(Guid lifeSituationId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/life-situation-up?lifeSituationId={lifeSituationId}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> LifeSituationUp(Guid lifeSituationId);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/life-situation-down?lifeSituationId={lifeSituationId}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> LifeSituationDown(Guid lifeSituationId);
    }
}