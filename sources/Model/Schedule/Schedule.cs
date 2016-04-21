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
        public virtual TimeSpan LiveClientInterval { get; set; }

        [Property]
        public virtual TimeSpan EarlyClientInterval { get; set; }

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

        [Property]
        public virtual bool OnlineOperatorsOnly { get; set; }

        [Property]
        public virtual int MaxOnlineOperators { get; set; }

        #endregion properties

        public override ValidationError[] Validate()
        {
            var errors = new List<ValidationError>(base.Validate());

            if (StartTime > FinishTime)
            {
                errors.Add(new ValidationError("Время начала не может быть больше времени окончания оказания услуги"));
            }

            if (LiveClientInterval <= TimeSpan.Zero)
            {
                errors.Add(new ValidationError("Время оказания услуги в живой очереди не может нулевым"));
            }

            if (EarlyClientInterval <= TimeSpan.Zero)
            {
                errors.Add(new ValidationError("Время оказания услуги по записи не может нулевым"));
            }

            if (RenderingMode == ServiceRenderingMode.AllRequests)
            {
                if (EarlyStartTime < StartTime)
                {
                    errors.Add(new ValidationError("Время начала предварительной записи не может быть меньше времени начала оказания услуги"));
                }

                if (EarlyFinishTime > FinishTime)
                {
                    errors.Add(new ValidationError("Время окончания предварительной записи не может быть больше времени окончания оказания услуги"));
                }

                if (EarlyStartTime > EarlyFinishTime)
                {
                    errors.Add(new ValidationError("Время начала предварительной записи не может быть больше времени окончания предварительной записи"));
                }
            }

            return errors.ToArray();
        }
    }
}