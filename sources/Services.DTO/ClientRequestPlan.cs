using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ClientRequestPlan
    {
        [DataMember]
        public ClientRequestLink ClientRequest { get; set; }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan FinishTime { get; set; }
    }

    [DataContract]
    public class ClientRequestPlanFull : ClientRequestPlan
    {
        [DataMember]
        public ClientRequestFull ClientRequest { get; set; }
    }
}