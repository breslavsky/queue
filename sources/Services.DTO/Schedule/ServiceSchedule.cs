using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ServiceSchedule : Schedule
    {
        [DataMember]
        public Service Service { get; set; }
    }
}