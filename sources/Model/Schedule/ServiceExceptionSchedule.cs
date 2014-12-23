using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "service_exception_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "ServiceExceptionScheduleToScheduleReference")]
    public class ServiceExceptionSchedule : ServiceSchedule
    {
        #region properties

        [Property(Index = "ScheduleDate")]
        public virtual DateTime ScheduleDate { get; set; }

        #endregion properties

        #region methods

        public override string ToString()
        {
            return string.Format("Исключающее расписание для услуги {0} на {1}", Service, ScheduleDate);
        }

        #endregion methods
    }
}