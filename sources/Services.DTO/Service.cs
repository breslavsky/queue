﻿using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    public class Service : IdentifiedEntity
    {
        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public int Priority { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Comment { get; set; }

        [DataMember]
        public string Tags { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public string Link { get; set; }

        [DataMember]
        public int MaxSubjects { get; set; }

        [DataMember]
        public int MaxClientRecalls { get; set; }

        [DataMember]
        public int MaxEarlyDays { get; set; }

        [DataMember]
        public bool IsPlanSubjects { get; set; }

        [DataMember]
        public bool ClientRequire { get; set; }

        [DataMember]
        public TimeSpan ClientCallDelay { get; set; }

        [DataMember]
        public TimeSpan TimeIntervalRounding { get; set; }

        [DataMember]
        public bool IsUseType { get; set; }

        [DataMember]
        public ClientRequestRegistrator LiveRegistrator { get; set; }

        [DataMember]
        public ClientRequestRegistrator EarlyRegistrator { get; set; }

        [DataMember]
        public string Color { get; set; }

        [DataMember]
        public float FontSize { get; set; }

        [DataMember]
        public long SortId { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public ServiceGroup ServiceGroup { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Code, Name);
        }
    }
}