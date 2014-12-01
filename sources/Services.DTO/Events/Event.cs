using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class EventLink : IdentifiedEntityLink { }

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

        public override IdentifiedEntityLink GetLink()
        {
            return new EventLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}