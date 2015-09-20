using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Queue.Services.Contracts
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface IServerTemplateTcpService : IServerTemplateService
    {
        [OperationContract]
        Task<string> GetTemplate(string app, string theme, string template);
    }
}