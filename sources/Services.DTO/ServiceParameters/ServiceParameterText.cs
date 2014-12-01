using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceParameterTextLink : IdentifiedEntityLink { }

    [DataContract]
    public class ServiceParameterText : ServiceParameter
    {
        public int MinLength { get; set; }

        public int MaxLength { get; set; }

        public override IdentifiedEntityLink GetLink()
        {
            return new ServiceParameterTextLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}