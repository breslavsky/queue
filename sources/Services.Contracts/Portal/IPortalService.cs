using Queue.Services.Common;
using Queue.Services.DTO;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Portal
{
    [ServiceContract]
    public partial interface IPortalService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        Stream Index();

        [OperationContract]
        [WebGet(UriTemplate = "/favicon.ico")]
        Stream Favicon();

        [OperationContract]
        [WebGet(UriTemplate = "/content/{*path}")]
        Stream GetContent(string path);

        [OperationContract]
        [WebGet(UriTemplate = "/config/default", ResponseFormat = WebMessageFormat.Json)]
        Task<DefaultConfig> GetDefaultConfig();

        [OperationContract]
        [WebGet(UriTemplate = "/config/portal", ResponseFormat = WebMessageFormat.Json)]
        Task<PortalConfig> GetPortalConfig();

        [OperationContract]
        [WebGet(UriTemplate = "/find-services?query={query}", ResponseFormat = WebMessageFormat.Json)]
        Task<Service[]> FindServices(string query);

        [OperationContract]
        [WebGet(UriTemplate = "/get-queue-plan-metric?year={year}&month={month}&day={day}&hour={hour}&minute={minute}&second={second}", ResponseFormat = WebMessageFormat.Json)]
        Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hour, int minute, int second);

        [OperationContract]
        [WebGet(UriTemplate = "/get-queue-plan-service-metric?year={year}&month={month}&day={day}&hour={hour}&minute={minute}&second={second}&serviceId={serviceId}", ResponseFormat = WebMessageFormat.Json)]
        Task<QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hour, int minute, int second, string serviceId);
    }
}