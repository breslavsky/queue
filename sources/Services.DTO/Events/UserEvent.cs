using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class UserEventLink : EventLink { }

    [DataContract]
    public class UserEvent : Event
    {
        [DataMember]
        public User User { get; set; }

        public override IdentifiedEntityLink GetLink()
        {
            return new UserEventLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}