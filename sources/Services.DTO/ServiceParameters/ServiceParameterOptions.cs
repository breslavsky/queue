using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public class ServiceParameterOptions : ServiceParameter
    {
        [DataMember]
        public string Options { get; set; }

        [DataMember]
        public bool IsMultiple { get; set; }
    }
}