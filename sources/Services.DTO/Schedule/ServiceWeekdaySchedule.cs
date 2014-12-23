using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceWeekdaySchedule : ServiceSchedule
    {
        public ServiceWeekdaySchedule()
            : base()
        {
        }

        [DataMember]
        public DayOfWeek DayOfWeek { get; set; }
    }
}