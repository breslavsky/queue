using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public abstract class IdentifiedEntity : Entity
    {
        [DataMember]
        public Guid Id { get; set; }

        public bool Empty()
        {
            return Id == Guid.Empty;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj.GetHashCode() == GetHashCode();
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}