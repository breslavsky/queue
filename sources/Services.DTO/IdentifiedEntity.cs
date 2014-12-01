using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public abstract class IdentifiedEntityLink
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Presentation { get; set; }

        public override string ToString()
        {
            return Presentation;
        }
    }

    [DataContract]
    public abstract class IdentifiedEntity : Entity
    {
        [DataMember]
        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public abstract IdentifiedEntityLink GetLink();
    }
}