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
    public partial interface IHttpService
    {
        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.INDEX)]
        Stream Index();

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.FAVICON)]
        Stream Favicon();

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.CLIENT_ACCESS_POLICY)]
        Stream ClientAccessPolicy();

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.GET_CONTENT)]
        Stream GetContent(string path);

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.GET_DEFAULT_CONFIG, ResponseFormat = WebMessageFormat.Json)]
        Task<DefaultConfig> GetDefaultConfig();

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.GET_PORTAL_CONFIG, ResponseFormat = WebMessageFormat.Json)]
        Task<PortalConfig> GetPortalConfig();

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.FIND_SERVICES, ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> FindServices(string filter);

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.GET_QUEUE_PLAN_METRIC, ResponseFormat = WebMessageFormat.Json)]
        Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hour, int minute, int second);

        [OperationContract]
        [WebGet(UriTemplate = HttpServiceMap.GET_QUEUE_PLAN_SERVICE_METRIC, ResponseFormat = WebMessageFormat.Json)]
        Task<QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hour, int minute, int second, string serviceId);
    }
}
