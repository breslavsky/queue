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
    public partial interface IMediaService
    {
        [OperationContract]
        [WebGet(UriTemplate = MediaServiceMap.Index)]
        string Index();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = MediaServiceMap.UploadMediaConfigFile)]
        void UploadMediaConfigFile(string mediaConfigFileId, Stream data);

        [OperationContract]
        [WebGet(UriTemplate = MediaServiceMap.LoadMediaConfigFile)]
        Stream LoadMediaConfigFile(string mediaConfigFileId);
    }
}
