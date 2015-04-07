using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public class Operator : User
    {
        [DataMember]
        public Workplace Workplace { get; set; }
    }
}