using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestEventLink : EventLink { }

    [DataContract]
    public class ClientRequestEvent : Event
    {
        [DataMember]
        public ClientRequest ClientRequest { get; set; }

        public override IdentifiedEntityLink GetLink()
        {
            return new ClientRequestEventLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}