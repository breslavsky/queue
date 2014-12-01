using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientLink : IdentifiedEntityLink { }

    [DataContract]
    public class Client : IdentifiedEntity
    {
        [DataMember]
        public DateTime RegisterDate { get; set; }

        [DataMember]
        public Guid SessionId { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Patronymic { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public string Identity { get; set; }

        [DataMember]
        public bool VIP { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Surname, Name).Trim();
        }

        public override IdentifiedEntityLink GetLink()
        {
            return new ClientLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }

    [DataContract]
    public class ClientFull : Client
    {
    }
}