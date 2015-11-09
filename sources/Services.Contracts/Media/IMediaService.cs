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

namespace Queue.Services.Contracts.Media
{
    [ServiceContract]
    public partial interface IMediaService
    {
        [OperationContract]
        [WebGet(UriTemplate = "/")]
        string Index();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/media-config/files/{mediaConfigFileId}/upload")]
        void UploadMediaConfigFile(string mediaConfigFileId, Stream data);

        [OperationContract]
        [WebGet(UriTemplate = "/media-config/files/{mediaConfigFileId}/load")]
        Stream LoadMediaConfigFile(string mediaConfigFileId);
    }
}