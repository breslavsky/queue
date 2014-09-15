using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "service_weekday_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "ServiceWeekdayScheduleToScheduleReference")]
    public class ServiceWeekdaySchedule : Schedule
    {
        public ServiceWeekdaySchedule()
            : base()
        {
            DayOfWeek = DayOfWeek.Monday;
        }

        #region properties

        [Property]
        public virtual DayOfWeek DayOfWeek { get; set; }

        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ServiceWeekdayScheduleToServiceReference")]
        public virtual Service Service { get; set; }
        
        #endregion properties        
        
        public override string ToString()
        {
            return string.Format("Регулярное расписание для услуги на {0}", DayOfWeek);
        }
    }
}