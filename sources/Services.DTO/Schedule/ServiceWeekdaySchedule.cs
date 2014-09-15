using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceWeekdaySchedule : Schedule
    {
        [DataMember]
        public DayOfWeek DayOfWeek { get; set; }

        [DataMember]
        public Service Service { get; set; }
    }
}