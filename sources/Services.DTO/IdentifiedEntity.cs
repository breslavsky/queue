using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class IdentifiedEntity : Entity
    {
        [DataMember]
        public Guid Id { get; set; }

        [DataMember]
        public string Presentation { get; set; }

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

        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Presentation) ? Id.ToString() : Presentation;
        }

        public T Cast<T>() where T : IdentifiedEntity
        {
            T instance = Activator.CreateInstance<T>();
            instance.Id = Id;
            instance.Presentation = Presentation;
            return instance;
        }
    }
}