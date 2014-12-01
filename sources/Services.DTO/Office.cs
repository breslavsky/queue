using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OfficeLink : IdentifiedEntityLink { }

    [DataContract]
    public class Office : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Endpoint { get; set; }

        [DataMember]
        public Guid SessionId { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override IdentifiedEntityLink GetLink()
        {
            return new OfficeLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}