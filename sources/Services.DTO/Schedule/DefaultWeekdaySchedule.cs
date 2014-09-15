using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class DefaultWeekdaySchedule : Schedule
    {
        [DataMember]
        public DayOfWeek DayOfWeek { get; set; }
    }
}