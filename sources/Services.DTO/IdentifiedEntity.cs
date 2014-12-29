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

    [DataContract]
    public class IdentifiedEntityLink : IdentifiedEntity
    {
        [DataMember]
        public string Presentation { get; set; }

        public T Cast<T>() where T : IdentifiedEntity
        {
            T instance = Activator.CreateInstance<T>();
            instance.Id = Id;
            return instance;
        }

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Presentation) ? Id.ToString() : Presentation;
        }
    }
}