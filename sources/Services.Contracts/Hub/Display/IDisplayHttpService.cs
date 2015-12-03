using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Hub
{
    [ServiceContract]
    public interface IDisplayHttpService : IDisplayService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/show-lines?deviceId={deviceId}&lines={lines}", ResponseFormat = WebMessageFormat.Json)]
        void ShowLines(byte deviceId, string lines);
    }
}