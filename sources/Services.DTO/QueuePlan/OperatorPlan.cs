using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public partial class QueuePlan
    {
        [DataContract]
        public class OperatorPlan
        {
            [DataMember]
            public Operator Operator { get; set; }

            [DataMember]
            public ClientRequestPlan[] ClientRequestPlans { get; set; }

            [DataMember]
            public OperatorInterruption[] Interruptions { get; set; }
        }
    }
}