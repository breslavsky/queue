using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(UserEvent))]
    [KnownType(typeof(ClientRequestEvent))]
    public class Event : IdentifiedEntity
    {
        [DataMember]
        public DateTime CreateDate { get; set; }
        [DataMember]
        public EventType Type { get; set; }
        [DataMember]
        public string Message { get; set; }

        public override string ToString()
        {
            return Message;
        }
    }
}
