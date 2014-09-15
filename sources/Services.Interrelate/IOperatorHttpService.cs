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

namespace Queue.Services.Interrelate
{
    [ServiceContract]
    public partial interface IOperatorHttpService
    {
        [OperationContract]
        [WebGet(UriTemplate = OperatorHttpServiceMap.INDEX)]
        Stream Index();
    }
}
