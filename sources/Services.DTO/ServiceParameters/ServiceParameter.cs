using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(ServiceParameterNumber))]
    [KnownType(typeof(ServiceParameterText))]
    [KnownType(typeof(ServiceParameterOptions))]
    public class ServiceParameter : IdentifiedEntity
    {
        [DataMember]
        public Service Service { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public bool IsRequire { get; set; }

        [DataMember]
        public string ToolTip { get; set; }

        [DataMember]
        public long SortId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}