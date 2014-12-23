using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceExceptionSchedule : ServiceSchedule
    {
        public ServiceExceptionSchedule()
            : base()
        {
        }

        [DataMember]
        public DateTime ScheduleDate { get; set; }
    }
}