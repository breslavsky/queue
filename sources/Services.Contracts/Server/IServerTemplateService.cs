using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract(Namespace = "http://queue.name/server-template")]
    public interface IServerTemplateService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/echo?message={message}", ResponseFormat = WebMessageFormat.Json)]
        Task<string> Echo(string message);

        [OperationContract]
        [WebGet(UriTemplate = "/heartbeat", ResponseFormat = WebMessageFormat.Json)]
        Task Heartbeat();

        [OperationContract]
        [WebGet(UriTemplate = "/{app}/{theme}/{template}")]
        Stream GetTemplate(string app, string theme, string template);
    }
}