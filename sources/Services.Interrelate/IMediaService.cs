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
    public partial interface IMediaService
    {
        [OperationContract]
        [WebGet(UriTemplate = MediaServiceMap.INDEX)]
        string Index();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = MediaServiceMap.UPLOAD_MEDIA_CONFIG_FILE)]
        void UploadMediaConfigFile(string mediaConfigFileId, Stream data);

        [OperationContract]
        [WebGet(UriTemplate = MediaServiceMap.LOAD_MEDIA_CONFIG_FILE)]
        Stream LoadMediaConfigFile(string mediaConfigFileId);
    }
}
