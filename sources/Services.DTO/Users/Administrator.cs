using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class AdministratorLink : UserLink { }

    [DataContract]
    public class Administrator : User
    {
        public override IdentifiedEntityLink GetLink()
        {
            return new AdministratorLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}