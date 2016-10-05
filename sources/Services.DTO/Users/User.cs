using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(Administrator))]
    [KnownType(typeof(Operator))]
    public class User : IdentifiedEntity
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
        public string Identity { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsMultisession { get; set; }

        [DataMember]
        public bool Online { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Surname, Name).Trim();
        }
    }
}