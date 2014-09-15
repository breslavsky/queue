using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class OperatorInterruption : IdentifiedEntity
    {
        [DataMember]
        public Operator Operator { get; set; }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan FinishTime { get; set; }
    }
}