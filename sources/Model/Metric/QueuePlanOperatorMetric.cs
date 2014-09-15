using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "metric_queue_plan_operator", ExtendsType = typeof(Metric), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "MetricId", ForeignKey = "QueuePlanOperatorMetricToMetricReference")]
    public class QueuePlanOperatorMetric : Metric
    {
        #region properties

        [ManyToOne(ClassType = typeof(Operator), Column = "ServiceId", ForeignKey = "QueuePlanOperatorMetricToServiceReference")]
        public virtual Operator Operator { get; set; }

        [Property]
        public virtual int Rendered { get; set; }

        [Property]
        public virtual TimeSpan RenderTime { get; set; }

        [Property]
        public virtual int Live { get; set; }

        [Property]
        public virtual int Early { get; set; }

        [Property]
        public virtual float Productivity { get; set; }

        #endregion properties
    }
}