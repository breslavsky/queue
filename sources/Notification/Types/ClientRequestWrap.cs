using Queue.Services.DTO;
using System;

namespace Queue.Notification
{
    public class ClientRequestWrap
    {
        public ClientRequest Request { get; set; }

        public DateTime Added { get; set; }
    }
}