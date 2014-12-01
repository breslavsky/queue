using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceParameterNumberLink : IdentifiedEntityLink { }

    [DataContract]
    public class ServiceParameterNumber : ServiceParameter
    {
        public override IdentifiedEntityLink GetLink()
        {
            return new ServiceParameterNumberLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}