using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class ScheduleLink : IdentifiedEntityLink { }

    [DataContract]
    [KnownType(typeof(DefaultWeekdaySchedule))]
    [KnownType(typeof(DefaultExceptionSchedule))]
    [KnownType(typeof(ServiceWeekdaySchedule))]
    [KnownType(typeof(ServiceExceptionSchedule))]
    public class Schedule : IdentifiedEntity
    {
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
        public TimeSpan ClientInterval { get; set; }

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

        public override IdentifiedEntityLink GetLink()
        {
            return new ScheduleLink
            {
                Id = Id,
                Presentation = ToString()
            };
        }
    }
}