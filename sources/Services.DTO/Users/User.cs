using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(AdministratorLink))]
    [KnownType(typeof(ManagerLink))]
    [KnownType(typeof(OperatorLink))]
    public class UserLink : IdentifiedEntityLink { }

    [DataContract]
    [KnownType(typeof(Administrator))]
    [KnownType(typeof(Manager))]
    [KnownType(typeof(Operator))]
    public abstract class User : IdentifiedEntity
    {
        [DataMember]
        public DateTime CreateDate { get; set; }

        [DataMember]
        public Guid SessionId { get; set; }

        [DataMember]
        public DateTime Heartbeat { get; set; }

        [DataMember]
        public string Surname { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Patronymic { get; set; }

        [DataMember]
        public string Email { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public bool Online { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Surname, Name).Trim();
        }

        public override IdentifiedEntityLink GetLink()
        {
            return new UserLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}