using Queue.Model.Common;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(DefaultConfig))]
    [KnownType(typeof(DesignConfig))]
    public abstract class Metric : IdentifiedEntity
    {
        [DataMember]
        public virtual int Year { get; set; }

        [DataMember]
        public virtual int Month { get; set; }

        [DataMember]
        public virtual int Day { get; set; }

        [DataMember]
        public virtual int Hour { get; set; }

        [DataMember]
        public virtual int Minute { get; set; }

        [DataMember]
        public virtual int Second { get; set; }
    }

    [DataContract]
    public class QueuePlanMetric : Metric
    {
        [DataMember]
        public int Rendered { get; set; }

        [DataMember]
        public int Waiting { get; set; }

        [DataMember]
        public int Live { get; set; }

        [DataMember]
        public int Early { get; set; }

        [DataMember]
        public TimeSpan RenderTime { get; set; }

        [DataMember]
        public TimeSpan WaitingTime { get; set; }

        [DataMember]
        public float Productivity { get; set; }
    }

    [DataContract]
    public class QueuePlanServiceMetric : Metric
    {
        [DataMember]
        public Service Service { get; set; }

        [DataMember]
        public int Rendered { get; set; }

        [DataMember]
        public int Waiting { get; set; }

        [DataMember]
        public int Live { get; set; }

        [DataMember]
        public int Early { get; set; }

        [DataMember]
        public TimeSpan RenderTime { get; set; }

        [DataMember]
        public TimeSpan WaitingTime { get; set; }

        [DataMember]
        public float Productivity { get; set; }
    }
}