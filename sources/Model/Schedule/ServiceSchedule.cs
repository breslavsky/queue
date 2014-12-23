using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    public abstract class ServiceSchedule : Schedule
    {
        #region properties

        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ServiceWeekdayScheduleToServiceReference")]
        public virtual Service Service { get; set; }

        #endregion properties
    }
}