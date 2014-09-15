using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Queue.Services.DTO
{
    public class ServiceFreeTime
    {
        [DataMember]
        public Service Service;

        [DataMember]
        public Schedule Schedule;

        [DataMember]
        public TimeSpan[] TimeIntervals;

        [DataMember]
        public int FreeTimeIntervals { get; set; }

        [DataMember]
        public string[] Report { get; set; }
    }
}