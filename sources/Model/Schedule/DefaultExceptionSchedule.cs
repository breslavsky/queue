using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    [JoinedSubclass(Table = "default_exception_schedule", ExtendsType = typeof(Schedule), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ScheduleId", ForeignKey = "DefaultExceptionScheduleToScheduleReference")]
    public class DefaultExceptionSchedule : Schedule
    {
        //TODO: см. выше
        public DefaultExceptionSchedule()
            : base()
        {
            ScheduleDate = DateTime.Now;
        }

        #region properties

        [Property(Index = "ScheduleDate")]
        public virtual DateTime ScheduleDate { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("Общее исключающее расписание на {0}", ScheduleDate);
        }
    }
}