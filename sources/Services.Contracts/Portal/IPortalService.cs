using Queue.Services.Common;
using Queue.Services.DTO;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract]
    public partial interface IPortalService
    {
        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.Index)]
        Stream Index();

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.Favicon)]
        Stream Favicon();

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.ClientAccessPolicy)]
        Stream ClientAccessPolicy();

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.GetContent)]
        Stream GetContent(string path);

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.GetDefaultConfig, ResponseFormat = WebMessageFormat.Json)]
        Task<DefaultConfig> GetDefaultConfig();

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.GetPortalConfig, ResponseFormat = WebMessageFormat.Json)]
        Task<PortalConfig> GetPortalConfig();

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.FindServices, ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> FindServices(string query);

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.GetQueuePlanMetric, ResponseFormat = WebMessageFormat.Json)]
        Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hour, int minute, int second);

        [OperationContract]
        [WebGet(UriTemplate = PortalServiceMap.GetQueuePlanServiceMetric, ResponseFormat = WebMessageFormat.Json)]
        Task<QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hour, int minute, int second, string serviceId);
    }
}