using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "metric_queue_plan", ExtendsType = typeof(Metric), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "MetricId", ForeignKey = "QueuePlanMetricToMetricReference")]
    public class QueuePlanMetric : Metric
    {
        #region properties

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