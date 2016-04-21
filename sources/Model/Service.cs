using Junte.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;

namespace Queue.Model
{
    [Class(Table = "service", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class Service : IdentifiedEntity
    {
        private const int NameLength = 1024 * 5;
        private const int DescriptionLength = 1024 * 500;
        private const int TagsLength = 1024 * 500;

        public Service()
        {
            SortId = DateTime.Now.Ticks;
        }

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

        [Property(Length = DescriptionLength)]
        public virtual string Description { get; set; }

        [Property]
        public virtual ClientRequestRegistrator EarlyRegistrator { get; set; }

        [Property]
        public virtual string Color { get; set; }

        [Property]
        public virtual float FontSize { get; set; }

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

        [Property]
        public virtual int MaxClientRecalls { get; set; }

        [NotNullNotEmpty(Message = "Название услуги не указано")]
        [Property(Length = NameLength)]
        public virtual string Name { get; set; }

        [Property]
        public virtual int Priority { get; set; }

        [ManyToOne(ClassType = typeof(ServiceGroup), Column = "ServiceGroupId", ForeignKey = "ServiceToServiceGroupReference")]
        public virtual ServiceGroup ServiceGroup { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        [Property(Length = TagsLength)]
        public virtual string Tags { get; set; }

        [Property]
        public virtual TimeSpan TimeIntervalRounding { get; set; }

        [Property]
        public virtual bool IsUseType { get; set; }

        public virtual ServiceStep GetFirstStep(ISession session)
        {
            return session.CreateCriteria<ServiceStep>()
                .Add(Restrictions.Eq("Service", this))
                .AddOrder(Order.Asc("SortId"))
                .SetMaxResults(1)
                .UniqueResult<ServiceStep>();
        }

        #endregion properties

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Code, Name);
        }
    }
}