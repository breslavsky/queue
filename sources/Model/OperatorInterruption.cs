using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [Class(Table = "operator_interruption", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class OperatorInterruption : IdentifiedEntity
    {
        #region properties

        [ManyToOne(ClassType = typeof(Operator), Column = "OperatorId", ForeignKey = "OperatorInterruptionToOperatorReference")]
        public virtual Operator Operator { get; set; }

        [Property]
        public virtual DayOfWeek DayOfWeek { get; set; }

        [Property]
        public virtual TimeSpan StartTime { get; set; }

        [Property]
        public virtual TimeSpan FinishTime { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("Перерыв с {0} до {1}", StartTime, FinishTime);
        }
    }
}