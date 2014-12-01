using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestParameterLink : IdentifiedEntityLink { }

    [DataContract]
    public class ClientRequestParameter : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Name, Value);
        }

        public override IdentifiedEntityLink GetLink()
        {
            return new ClientRequestParameterLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}