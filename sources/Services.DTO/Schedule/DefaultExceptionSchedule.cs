using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class DefaultExceptionSchedule : Schedule
    {
        public DefaultExceptionSchedule()
            : base()
        {
        }

        [DataMember]
        public DateTime ScheduleDate { get; set; }
    }
}