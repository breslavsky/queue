using NHibernate.Mapping.Attributes;
using System;

namespace Queue.Model
{
    public abstract class ExceptionSchedule : Schedule
    {
        public ExceptionSchedule()
            : base()
        {
            ScheduleDate = DateTime.Now;
        }

        #region properties

        [Property(Index = "ScheduleDate")]
        public virtual DateTime ScheduleDate { get; set; }

        #endregion properties

        #region methods

        public override string ToString()
        {
            return string.Format("Общее исключающее расписание на {0}", ScheduleDate);
        }

        #endregion methods
    }
}