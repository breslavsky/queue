﻿using Queue.Model.Common;
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
            LiveClientInterval = new TimeSpan(0, 10, 0);
            Intersection = TimeSpan.Zero;
            MaxClientRequests = byte.MaxValue;
            RenderingMode = ServiceRenderingMode.AllRequests;
            EarlyStartTime = new TimeSpan(10, 0, 0);
            EarlyFinishTime = new TimeSpan(18, 0, 0);
            EarlyReservation = 50;
            MaxOnlineOperators = 1;
        }

        [DataMember]
        public TimeSpan StartTime { get; set; }

        [DataMember]
        public TimeSpan FinishTime { get; set; }

        [DataMember]
        public bool IsWorked { get; set; }

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

        [DataMember]
        public int MaxOnlineOperators { get; set; }

        [DataMember]
        public bool OnlineOperatorsOnly { get; set; }
    }
}