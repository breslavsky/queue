using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "service_weekday_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "ServiceWeekdayScheduleToScheduleReference")]
    public class ServiceWeekdaySchedule : ServiceSchedule
    {
        public ServiceWeekdaySchedule()
            : base()
        {
            DayOfWeek = DayOfWeek.Monday;
        }

        #region properties

        [Property]
        public virtual DayOfWeek DayOfWeek { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("Регулярное расписание для услуги {0} на {1}", Service, DayOfWeek);
        }
    }
}