using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
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
    }
}