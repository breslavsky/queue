﻿using Junte.Data.Common;
using Junte.Data.NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

namespace Queue.Model
{
    [Class(Table = "service", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class Service : IdentifiedEntity
    {
        public Service()
        {
            SortId = DateTime.Now.Ticks;
        }

        #region fields

        private IList<ServiceExceptionSchedule> exceptionSchedule = new List<ServiceExceptionSchedule>();
        private IList<ServiceParameter> parameters = new List<ServiceParameter>();
        private IList<ServiceWeekdaySchedule> regularSchedule = new List<ServiceWeekdaySchedule>();
        private IList<ServiceStep> steps = new List<ServiceStep>();

        #endregion fields

        #region properties

        [Property]
        public virtual TimeSpan ClientCallDelay { get; set; }

        [Property]
        public virtual bool ClientRequire { get; set; }

        [NotNullNotEmpty(Message = "Код услуги не указан")]
        [Property]
        public virtual string Code { get; set; }

        [Property]
        public virtual string Comment { get; set; }

        [Property(Length = DataLength._500K)]
        public virtual string Description { get; set; }

        [Property]
        public virtual ClientRequestRegistrator EarlyRegistrator { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }

        [Property]
        public virtual bool IsPlanSubjects { get; set; }

        [Property]
        public virtual string Link { get; set; }

        [Property]
        public virtual ClientRequestRegistrator LiveRegistrator { get; set; }

        [Property]
        public virtual int MaxEarlyDays { get; set; }

        [Property]
        public virtual int MaxSubjects { get; set; }

        [Length(Message = "Название услуги не указано")]
        [Property(Length = 1000)]
        public virtual string Name { get; set; }

        [Property]
        public virtual int Priority { get; set; }

        [ManyToOne(ClassType = typeof(ServiceGroup), Column = "ServiceGroupId", ForeignKey = "ServiceToServiceGroupReference")]
        public virtual ServiceGroup ServiceGroup { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        [Property(Length = DataLength._1K)]
        public virtual string Tags { get; set; }

        [Property]
        public virtual TimeSpan TimeIntervalRounding { get; set; }

        [Property]
        public virtual ServiceType Type { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Code, Name);
        }
    }
}