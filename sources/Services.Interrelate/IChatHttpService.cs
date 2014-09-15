using Queue.Data.Model.DTO;
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
    public partial interface IChatHttpService
    {
        [OperationContract]
        [WebGet(UriTemplate = ChatServiceUri.INDEX)]
        Stream Index();

        [OperationContract]
        [WebInvoke(Method = "POST", BodyStyle = WebMessageBodyStyle.WrappedRequest, UriTemplate = ChatServiceUri.GET_CHAT_MESSAGES, ResponseFormat = WebMessageFormat.Json)]
        object GetChatMessage(Stream stream);
    }
}
