using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

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
        public virtual OperatorInterruptionType Type { get; set; }

        [Property]
        public virtual DateTime TargetDate { get; set; }

        [Property]
        public virtual DayOfWeek DayOfWeek { get; set; }

        [Property]
        public virtual TimeSpan StartTime { get; set; }

        [Property]
        public virtual TimeSpan FinishTime { get; set; }

        [Property]
        public virtual ServiceRenderingMode ServiceRenderingMode { get; set; }

        [Property]
        public virtual int WeekFold { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("Перерыв с {0} до {1}", StartTime, FinishTime);
        }

        public override ValidationError[] Validate()
        {
            var errors = new List<ValidationError>(base.Validate());

            if (StartTime > FinishTime)
            {
                errors.Add(new ValidationError("Время начала не может быть больше времени окончания перерыва"));
            }

            return errors.ToArray();
        }
    }
}