using Queue.Services.Common;
using Queue.Services.DTO;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Portal
{
    [ServiceContract]
    public partial interface IPortalClientService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        Stream Index();

        [OperationContract]
        [WebGet(UriTemplate = "/login?email={email}&password={password}", ResponseFormat = WebMessageFormat.Json)]
        Task<Client> Login(string email, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = "/ulogin")]
        Task<Stream> ULogin(Stream stream);

        [OperationContract]
        [WebGet(UriTemplate = "/restore-password?email={email}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> RestorePassword(string email);

        [OperationContract]
        [WebGet(UriTemplate = "/get-profile", ResponseFormat = WebMessageFormat.Json)]
        Client GetProfile();

        [OperationContract]
        [WebGet(UriTemplate = "/edit-profile?surname={surname}&name={name}&patronymic={patronymic}&mobile={mobile}", ResponseFormat = WebMessageFormat.Json)]
        Task<Client> EditProfile(string surname, string name, string patronymic, string mobile);

        [OperationContract]
        [WebGet(UriTemplate = "/get-requests", ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest[]> GetRequests();

        [OperationContract]
        [WebGet(UriTemplate = "/cancel-request?requestId={requestId}", ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> CancelRequest(string requestId);

        [OperationContract]
        [WebGet(UriTemplate = "/send-pin-to-email?email={email}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> SendPINToEmail(string email);

        [OperationContract]
        [WebGet(UriTemplate = "/check-pin?email={email}&PIN={PIN}", ResponseFormat = WebMessageFormat.Json)]
        Task<bool> CheckPIN(string email, string PIN);

        [OperationContract]
        [WebGet(UriTemplate = "/register?email={email}&PIN={PIN}&surname={surname}&name={name}&patronymic={patronymic}&mobile={mobile}", ResponseFormat = WebMessageFormat.Json)]
        Task<Client> Register(string email, string PIN, string surname, string name, string patronymic, string mobile);

        [OperationContract]
        [WebGet(UriTemplate = "/root-service-groups", ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceGroup[]> GetRootServiceGroups();

        [OperationContract]
        [WebGet(UriTemplate = "/service-group/{serviceGroupId}/child-groups", ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceGroup[]> GetServiceGroups(string serviceGroupId);

        [OperationContract]
        [WebGet(UriTemplate = "/root-services", ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> GetRootServices();

        [OperationContract]
        [WebGet(UriTemplate = "/service-group/{serviceGroupId}/services", ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> GetServices(string serviceGroupId);

        [OperationContract]
        [WebGet(UriTemplate = "/queue-plan/{planDate}/{queueType}/service/{serviceId}/free-time", ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceFreeTime> GetServiceFreeTime(string planDate, string queueType, string serviceId);

        [OperationContract]
        [WebGet(UriTemplate = "/add-request?serviceId={serviceId}&requestDate={requestDate}&requestTime={requestTime}&subjects={subjects}", ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> AddRequest(string serviceId, string requestDate, string requestTime, string subjects);

        [OperationContract]
        [WebGet(UriTemplate = "/request/{requestId}/coupon", ResponseFormat = WebMessageFormat.Json)]
        Task<Stream> GetRequestCoupon(string requestId);
    }
}