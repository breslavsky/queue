using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceParameterOptionsLink : IdentifiedEntityLink { }

    [DataContract]
    public class ServiceParameterOptions : ServiceParameter
    {
        [DataMember]
        public string Options { get; set; }

        [DataMember]
        public bool IsMultiple { get; set; }

        public override IdentifiedEntityLink GetLink()
        {
            return new ServiceParameterOptionsLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}