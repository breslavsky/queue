using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceStepLink : IdentifiedEntityLink { }

    [DataContract]
    public class ServiceStep : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override IdentifiedEntityLink GetLink()
        {
            return new ServiceStepLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }

    [DataContract]
    public class ServiceStepFull : ServiceStep
    {
    }
}