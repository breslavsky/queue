using Junte.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;
using System.Collections.Generic;

namespace Queue.Model
{
    [Class(Table = "life_situation", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class LifeSituation : IdentifiedEntity
    {
        private const int DescriptionLength = 1024 * 500;

        public LifeSituation()
        {
            SortId = DateTime.Now.Ticks;
        }

        #region properties

        [NotNullNotEmpty(Message = "Код жизненной ситуации не указан")]
        [Property]
        public virtual string Code { get; set; }

        [Property]
        public virtual string Comment { get; set; }

        [Property(Length = DescriptionLength)]
        public virtual string Description { get; set; }

        [Property]
        public virtual string Color { get; set; }

        [Property]
        public virtual float FontSize { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }

        [NotNullNotEmpty(Message = "Название услуги не указано")]
        [Property(Length = 1000)]
        public virtual string Name { get; set; }

        [ManyToOne(ClassType = typeof(LifeSituationGroup), Column = "LifeSituationGroupId", ForeignKey = "LifeSituationToLifeSituationGroupReference")]
        public virtual LifeSituationGroup LifeSituationGroup { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        [NotNull(Message = "Для жизненной ситуации не указана услуга")]
        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "LifeSituationToServiceReference")]
        public virtual Service Service { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("[{0}] {1}", Code, Name);
        }
    }
}