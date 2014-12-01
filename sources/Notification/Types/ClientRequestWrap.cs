using Queue.Services.DTO;
using System;

namespace Queue.Notification.Types
{
    public class ClientRequestWrap
    {
        public ClientRequestFull Request { get; set; }

        public DateTime Added { get; set; }
    }
}