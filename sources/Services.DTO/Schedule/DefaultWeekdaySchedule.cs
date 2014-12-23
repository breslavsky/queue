using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class DefaultWeekdaySchedule : Schedule
    {
        public DefaultWeekdaySchedule()
            : base()
        {
        }

        [DataMember]
        public DayOfWeek DayOfWeek { get; set; }
    }
}