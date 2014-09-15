using Junte.WCF.Common;
using log4net;
using Queue.Data.Model.DTO;
using Queue.Data.Model.Common;
using Queue.Services.Common;
using Queue.Services.Interrelate;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web;

namespace Queue.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    public class ChatHttpService : HttpService, IChatHttpService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ChatHttpService));

        public ChatHttpService(DuplexChannelBuilder<IRemoteService> channelBuilder, Administrator currentAdministrator)
            : base(channelBuilder, currentAdministrator)
        {

        }

        public override Stream Index()
        {
            return GetContent("chat\\index.html");
        }

        private List<ChatMessage> chatMessages = new List<ChatMessage>();

        public class ChatMessage
        {

        }

        public object GetChatMessage(Stream stream)
        {
            using (StreamReader reader = new StreamReader(stream))
            {
                string body = reader.ReadToEnd();

                NameValueCollection parameters = HttpUtility.ParseQueryString(body);
            }

            return new ChatMessage();
        }
    }
}
