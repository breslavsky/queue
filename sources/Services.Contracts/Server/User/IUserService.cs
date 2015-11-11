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
    [ServiceContract(Namespace = "http://queue.name/server/user")]
    public interface IUserService : IStandardServerService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/identify?identity={identity}", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> Identify(string identity);

        [OperationContract]
        [WebGet(UriTemplate = "/open-session?sessionId={sessionId}", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> OpenUserSession(Guid sessionId);

        [OperationContract]
        [WebGet(UriTemplate = "/user-heartbeat", ResponseFormat = WebMessageFormat.Json)]
        Task UserHeartbeat();

        [OperationContract]
        [WebGet(UriTemplate = "/get-users", ResponseFormat = WebMessageFormat.Json)]
        Task<User[]> GetUsers();

        [OperationContract]
        [WebGet(UriTemplate = "/get-user?userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<User> GetUser(Guid userId);

        [OperationContract]
        [WebGet(UriTemplate = "/get-user-links?userRole={userRole}", ResponseFormat = WebMessageFormat.Json)]
        Task<IdentifiedEntityLink[]> GetUserLinks(UserRole userRole);

        [OperationContract]
        [WebGet(UriTemplate = "/get-redirect-operators-links", ResponseFormat = WebMessageFormat.Json)]
        Task<IdentifiedEntityLink[]> GetRedirectOperatorsLinks();

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/login?userId={userId}&password={password}", ResponseFormat = WebMessageFormat.Json)]
        Task<User> UserLogin(Guid userId, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebGet(UriTemplate = "/change-user-password?userId={userId}&password={password}", ResponseFormat = WebMessageFormat.Json)]
        Task ChangeUserPassword(Guid userId, string password);

        [OperationContract]
        [FaultContract(typeof(ObjectNotFoundFault))]
        [WebInvoke(Method = "PUT", UriTemplate = "/edit-administrator", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        Task<Administrator> EditAdministrator(Administrator administrator);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "/edit-operator", BodyStyle = WebMessageBodyStyle.Wrapped, ResponseFormat = WebMessageFormat.Json)]
        [FaultContract(typeof(ObjectNotFoundFault))]
        Task<Operator> EditOperator(Operator user);

        [OperationContract]
        [WebGet(UriTemplate = "/delete?userId={userId}", ResponseFormat = WebMessageFormat.Json)]
        Task DeleteUser(Guid userId);
    }
}