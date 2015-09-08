using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract(Namespace = "http://queue.name/hub-quality")]
    public interface IHubQualityService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/echo?message={message}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> Echo(string message);

        [OperationContract]
        [WebGet(UriTemplate = "/get-drivers", ResponseFormat = WebMessageFormat.Json)]
        Task<string[]> GetDrivers();

        [OperationContract]
        [WebGet(UriTemplate = "/enable?deviceId={deviceId}", ResponseFormat = WebMessageFormat.Json)]
        Task Enable(byte deviceId);

        [OperationContract]
        [WebGet(UriTemplate = "/disable?deviceId={deviceId}", ResponseFormat = WebMessageFormat.Json)]
        Task Disable(byte deviceId);

        [OperationContract]
        [WebGet(UriTemplate = "/get-answers", ResponseFormat = WebMessageFormat.Json)]
        Task<Dictionary<byte, int>> GetAnswers();
    }
}