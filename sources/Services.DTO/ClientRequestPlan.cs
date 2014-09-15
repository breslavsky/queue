using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestPlan
    {
        [DataMember]
        public ClientRequest ClientRequest { get; set; }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan FinishTime { get; set; }
    }
}