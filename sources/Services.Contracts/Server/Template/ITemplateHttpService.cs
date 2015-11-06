using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Server
{
    [ServiceContract]
    public interface ITemplateHttpService : ITemplateService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/{app}/{theme}/{template}")]
        Stream GetTemplate(string app, string theme, string template);
    }
}