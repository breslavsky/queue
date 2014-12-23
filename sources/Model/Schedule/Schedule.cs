using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

namespace Queue.Model
{
    [Class(Table = "schedule", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public abstract class Schedule : IdentifiedEntity
    {
        #region properties

        [Property]
        public virtual TimeSpan StartTime { get; set; }

        [Property]
        public virtual TimeSpan FinishTime { get; set; }

        [Property]
        public virtual bool IsWorked { get; set; }

        [Property]
        public virtual bool IsInterruption { get; set; }

        [Property]
        public virtual TimeSpan InterruptionStartTime { get; set; }

        [Property]
        public virtual TimeSpan InterruptionFinishTime { get; set; }

        [Property]
        public virtual TimeSpan ClientInterval { get; set; }

        [Property]
        public virtual TimeSpan Intersection { get; set; }

        [Property]
        public virtual int MaxClientRequests { get; set; }

        [Property]
        public virtual ServiceRenderingMode RenderingMode { get; set; }

        [Property]
        public virtual TimeSpan EarlyStartTime { get; set; }

        [Property]
        public virtual TimeSpan EarlyFinishTime { get; set; }

        [Property]
        public virtual int EarlyReservation { get; set; }

        [Property]
        public virtual int Version { get; set; }

        #endregion properties

        public override ValidationError[] Validate()
        {
            var errors = base.Validate();

            var result = new List<ValidationError>(errors);

            if (StartTime > FinishTime)
            {
                result.Add(new ValidationError("Время начала не может быть больше времени окончания оказания услуги"));
            }

            if (ClientInterval <= TimeSpan.Zero)
            {
                result.Add(new ValidationError("Время оказания услуги не может нулевым"));
            }

            if (IsInterruption)
            {
                if (InterruptionStartTime < StartTime)
                {
                    result.Add(new ValidationError("Время начала перерыва должно быть больше времени начала оказания услуги"));
                }

                if (InterruptionFinishTime > FinishTime)
                {
                    result.Add(new ValidationError("Время окончания перерыва должно быть меньше времени окончания оказания услуги"));
                }

                if (InterruptionStartTime > InterruptionFinishTime)
                {
                    result.Add(new ValidationError("Время начала перерыва должно быть меньше времени окончания перерыва", "InterruptionStartTime"));
                }
            }

            if (RenderingMode == ServiceRenderingMode.AllRequests)
            {
                if (EarlyStartTime < StartTime)
                {
                    result.Add(new ValidationError("Время начала предварительной записи не может быть меньше времени начала оказания услуги"));
                }

                if (EarlyFinishTime > FinishTime)
                {
                    result.Add(new ValidationError("Время окончания предварительной записи не может быть больше времени окончания оказания услуги"));
                }

                if (EarlyStartTime > EarlyFinishTime)
                {
                    result.Add(new ValidationError("Время начала предварительной записи не может быть больше времени окончания предварительной записи"));
                }
            }

            return result.ToArray();
        }
    }
}