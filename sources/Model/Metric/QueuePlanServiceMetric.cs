using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "metric_queue_plan_service", ExtendsType = typeof(Metric), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "MetricId", ForeignKey = "QueuePlanServiceMetricToMetricReference")]
    public class QueuePlanServiceMetric : Metric
    {
        #region properties

        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "QueuePlanServiceMetricToServiceReference")]
        public virtual Service Service { get; set; }

        [Property]
        public virtual int Rendered { get; set; }

        [Property]
        public virtual int Waiting { get; set; }

        [Property]
        public virtual int Live { get; set; }

        [Property]
        public virtual int Early { get; set; }

        [Property]
        public virtual TimeSpan RenderTime { get; set; }

        [Property]
        public virtual TimeSpan WaitingTime { get; set; }

        [Property]
        public virtual float Productivity { get; set; }

        #endregion properties
    }
}