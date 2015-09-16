using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract(Namespace = "http://queue.name/hub-display")]
    public interface IHubDisplayService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/echo?message={message}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> Echo(string message);

        [OperationContract]
        [WebGet(UriTemplate = "/heartbeat", ResponseFormat = WebMessageFormat.Json)]
        Task Heartbeat();

        [OperationContract]
        [WebGet(UriTemplate = "/get-drivers", ResponseFormat = WebMessageFormat.Json)]
        Task<string[]> GetDrivers();

        [OperationContract]
        [WebGet(UriTemplate = "/show-text?deviceId={deviceId}&text={text}", ResponseFormat = WebMessageFormat.Json)]
        Task ShowText(byte deviceId, string text);
    }
}