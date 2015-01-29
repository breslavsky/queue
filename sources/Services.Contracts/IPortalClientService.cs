using Queue.Services.Common;
using Queue.Services.DTO;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract]
    public partial interface IPortalClientService
    {
        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.Index)]
        Stream Index();

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.Login, ResponseFormat = WebMessageFormat.Json)]
        Task<Client> Login(string email, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = PortalClientServiceMap.ClientULogin)]
        Task<Stream> ULogin(Stream stream);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.RestorePassword, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> RestorePassword(string email);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetProfile, ResponseFormat = WebMessageFormat.Json)]
        Client GetProfile();

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.EditProfile, ResponseFormat = WebMessageFormat.Json)]
        Task<Client> EditProfile(string surname, string name, string patronymic, string mobile);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetRequests, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest[]> GetRequests();

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.CancelRequest, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> CancelRequest(string requestId);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.SendPINToEmail, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> SendPINToEmail(string email);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.CheckPIN, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> CheckPIN(string email, string PIN);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.Register, ResponseFormat = WebMessageFormat.Json)]
        Task<Client> Register(string email, string PIN, string surname, string name, string patronymic, string mobile);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetRootServiceGroups, ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceGroup[]> GetRootServiceGroups();

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetServiceGroups, ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceGroup[]> GetServiceGroups(string serviceGroupId);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetRootServices, ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> GetRootServices();

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetServices, ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> GetServices(string serviceGroupId);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetServiceFreeTime, ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceFreeTime> GetServiceFreeTime(string planDate, string queueType, string serviceId);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.AddRequest, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> AddRequest(string serviceId, string requestDate, string requestTime, string subjects);

        [OperationContract]
        [WebGet(UriTemplate = PortalClientServiceMap.GetRequestCoupon, ResponseFormat = WebMessageFormat.Json)]
        Task<Stream> GetRequestCoupon(string requestId);
    }
}