using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(DefaultWeekdaySchedule))]
    [KnownType(typeof(DefaultExceptionSchedule))]
    [KnownType(typeof(ServiceWeekdaySchedule))]
    [KnownType(typeof(ServiceExceptionSchedule))]
    public class Schedule : IdentifiedEntity
    {
        public Schedule()
        {
            StartTime = new TimeSpan(10, 0, 0);
            FinishTime = new TimeSpan(18, 0, 0);
            IsWorked = true;
            IsInterruption = true;
            InterruptionStartTime = new TimeSpan(12, 0, 0);
            InterruptionFinishTime = new TimeSpan(13, 0, 0);
            LiveClientInterval = new TimeSpan(0, 10, 0);
            Intersection = TimeSpan.Zero;
            MaxClientRequests = byte.MaxValue;
            RenderingMode = ServiceRenderingMode.AllRequests;
            EarlyStartTime = new TimeSpan(10, 0, 0);
            EarlyFinishTime = new TimeSpan(18, 0, 0);
            EarlyReservation = 50;
        }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan FinishTime { get; set; }

        [DataMember]
        public bool IsWorked { get; set; }

        [DataMember]
        public bool IsInterruption { get; set; }

        [DataMember]
        public TimeSpan InterruptionStartTime { get; set; }

        [DataMember]
        public TimeSpan InterruptionFinishTime { get; set; }

        [DataMember]
        public TimeSpan LiveClientInterval { get; set; }

        [DataMember]
        public TimeSpan EarlyClientInterval { get; set; }

        [DataMember]
        public TimeSpan Intersection { get; set; }

        [DataMember]
        public int MaxClientRequests { get; set; }

        [DataMember]
        public ServiceRenderingMode RenderingMode { get; set; }

        [DataMember]
        public TimeSpan EarlyStartTime { get; set; }

        [DataMember]
        public TimeSpan EarlyFinishTime { get; set; }

        [DataMember]
        public int EarlyReservation { get; set; }
    }
}