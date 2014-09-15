using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceStep : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}