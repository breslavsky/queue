using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "default_weekday_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "DefaultWeekdayScheduleToScheduleReference")]
    public class DefaultWeekdaySchedule : Schedule
    {
        #region properties

        [Property(Index = "DayOfWeek")]
        public virtual DayOfWeek DayOfWeek { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("Общее регулярное расписание на {0}", DayOfWeek);
        }
    }
}