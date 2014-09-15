using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceExceptionSchedule : Schedule
    {
        [DataMember]
        public DateTime ScheduleDate { get; set; }

        [DataMember]
        public Service Service { get; set; }
    }
}