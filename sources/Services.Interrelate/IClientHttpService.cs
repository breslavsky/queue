using Queue.Services.DTO;
using Queue.Services.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Interrelate
{
    [ServiceContract]
    public partial interface IClientHttpService
    {
        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.INDEX)]
        Stream Index();

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.LOGIN, ResponseFormat = WebMessageFormat.Json)]
        Task<Client> Login(string email, string password);

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = ClientHttpServiceMap.CLIENT_ULOGIN)]
        Task<Stream> ULogin(Stream stream);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.RESTORE_PASSWORD, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> RestorePassword(string email);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_PROFILE, ResponseFormat = WebMessageFormat.Json)]
        Client GetProfile();

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.EDIT_PROFILE, ResponseFormat = WebMessageFormat.Json)]
        Task<Client> EditProfile(string email, string surname, string name, string patronymic, string mobile);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_REQUESTS, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest[]> GetRequests();

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.CANCEL_REQUEST, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> CancelRequest(string requestId);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.SEND_PIN_TO_EMAIL, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> SendPINToEmail(string email);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.CHECK_PIN, ResponseFormat = WebMessageFormat.Json)]
        Task<bool> CheckPIN(string email, string PIN);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.REGISTER, ResponseFormat = WebMessageFormat.Json)]
        Task<Client> Register(string email, string PIN, string surname, string name, string patronymic, string mobile);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_ROOT_SERVICE_GROUPS, ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceGroup[]> GetRootServiceGroups();

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_SERVICE_GROUPS, ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceGroup[]> GetServiceGroups(string serviceGroupId);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_SERVICES, ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> GetServices(string serviceGroupId);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_FREE_TIME, ResponseFormat = WebMessageFormat.Json)]
        Task<ServiceFreeTime> GetFreeTime(string planDate, string queueType, string serviceId);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.ADD_REQUEST, ResponseFormat = WebMessageFormat.Json)]
        Task<ClientRequest> AddRequest(string serviceId, string requestDate, string requestTime, string subjects);

        [OperationContract]
        [WebGet(UriTemplate = ClientHttpServiceMap.GET_REQUEST_COUPON, ResponseFormat = WebMessageFormat.Json)]
        Task<Stream> GetRequestCoupon(string requestId);
    }
}
