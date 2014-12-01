using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ManagerLink : UserLink { }

    [DataContract]
    public class Manager : User
    {
        public override IdentifiedEntityLink GetLink()
        {
            return new ManagerLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}