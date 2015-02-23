using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class AdditionalService : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Measure { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}