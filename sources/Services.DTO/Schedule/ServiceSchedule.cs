using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceSchedule : Schedule
    {
        public ServiceSchedule()
            : base()
        {
        }

        [DataMember]
        public Service Service { get; set; }
    }
}