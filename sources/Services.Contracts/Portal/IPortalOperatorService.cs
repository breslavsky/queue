using Queue.Services.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Contracts.Portal
{
    [ServiceContract]
    public partial interface IPortalOperatorService
    {
        [OperationContract]
        [WebGet(UriTemplate = PortalOperatorServiceMap.Index)]
        Stream Index();
    }
}