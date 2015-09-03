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

namespace Queue.Services.Contracts
{
    [ServiceContract]
    public partial interface IPortalOperatorService
    {
        [OperationContract]
        [WebGet(UriTemplate = PortalOperatorServiceMap.Index)]
        Stream Index();
    }
}
