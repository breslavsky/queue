using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Common
{
    public class FaultErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            return error is FaultException;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            fault = Message.CreateMessage(version, string.Empty, error.Message);

            var messageProperty = new HttpResponseMessageProperty
            {
                StatusCode = HttpStatusCode.BadRequest,
                StatusDescription = "Bad Request",
            };
            messageProperty.Headers[HttpResponseHeader.ContentType] = "text/plain";

            fault.Properties.Add(HttpResponseMessageProperty.Name, messageProperty);
        }
    }
}