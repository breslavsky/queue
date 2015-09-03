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
    [ServiceContract(Namespace = "http://queue.name/hub-display")]
    public interface IHubDisplayService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/get-drivers", ResponseFormat = WebMessageFormat.Json)]
        Task<string[]> GetDrivers();

        [OperationContract]
        [WebGet(UriTemplate = "/echo?message={message}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> Echo(string message);

        [OperationContract]
        [WebGet(UriTemplate = "/show-number?deviceId={deviceId}&number={number}", ResponseFormat = WebMessageFormat.Json)]
        Task ShowNumber(byte deviceId, short number);
    }
}