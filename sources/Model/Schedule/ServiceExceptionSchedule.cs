using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "service_exception_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "ServiceExceptionScheduleToScheduleReference")]
    public class ServiceExceptionSchedule : Schedule
    {
        public ServiceExceptionSchedule()
            : base()
        {
            ScheduleDate = DateTime.MinValue;
        }

        #region properties

        [Property]
        public virtual DateTime ScheduleDate { get; set; }

        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ServiceExceptionScheduleToServiceReference")]
        public virtual Service Service { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("Исключающее расписание для услуги на {0}", ScheduleDate);
        }
    }
}