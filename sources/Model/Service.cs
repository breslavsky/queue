using Junte.Data.Common;
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
        #region fields

        private IList<ServiceWeekdaySchedule> regularSchedule = new List<ServiceWeekdaySchedule>();
        private IList<ServiceExceptionSchedule> exceptionSchedule = new List<ServiceExceptionSchedule>();
        private IList<ServiceParameter> parameters = new List<ServiceParameter>();
        private IList<ServiceStep> steps = new List<ServiceStep>();

        #endregion fields

        public Service()
        {
            Code = "0.0";
            Name = "Новая услуга";
            MaxSubjects = 1;
            MaxEarlyDays = 30;
            TimeIntervalRounding = TimeSpan.FromMinutes(5);
            SortId = DateTime.Now.Ticks;
        }

        #region properties

        [Property]
        public virtual bool IsActive { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        [Length(Min = 1, Max = 15, Message = "Поле (код услуги) должно быть более 1 и менее 15 символов")]
        [Property]
        public virtual string Code { get; set; }

        [Property]
        public virtual int Priority { get; set; }

        [Length(Min = 1, Max = 1000, Message = "Поле (название услуги) должно быть больше 1 и менее 1000 символов")]
        [Property(Length = 1000)]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Comment { get; set; }

        [Property(Length = DataLength._1K)]
        public virtual string Tags { get; set; }

        [Property(Length = DataLength._500K)]
        public virtual string Description { get; set; }

        [Property]
        public virtual string Link { get; set; }

        [Property]
        public virtual int MaxSubjects { get; set; }

        [Property]
        public virtual int MaxEarlyDays { get; set; }

        [Property]
        public virtual bool IsPlanSubjects { get; set; }

        [Property]
        public virtual TimeSpan ClientCallDelay { get; set; }

        [Property]
        public virtual bool ClientRequire { get; set; }

        [Property]
        public virtual TimeSpan TimeIntervalRounding { get; set; }

        [ManyToOne(ClassType = typeof(ServiceGroup), Column = "ServiceGroupId", ForeignKey = "ServiceToServiceGroupReference")]
        public virtual ServiceGroup ServiceGroup { get; set; }

        [Property]
        public virtual ServiceType Type { get; set; }

        [Property]
        public virtual ClientRequestRegistrator LiveRegistrator { get; set; }

        [Property]
        public virtual ClientRequestRegistrator EarlyRegistrator { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Code, Name);
        }
    }
}