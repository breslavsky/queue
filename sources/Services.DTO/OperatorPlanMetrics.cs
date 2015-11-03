using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    public class OperatorPlanMetrics
    {
        [DataMember]
        public Operator Operator { get; set; }

        [DataMember]
        public int LastPosition { get; set; }

        [DataMember]
        public TimeSpan DailyWorkload { get; set; }

        [DataMember]
        public TimeSpan PlaningWorkload { get; set; }

        [DataMember]
        public int Standing { get; set; }
    }
}