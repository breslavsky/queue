using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "life_situation_group", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class LifeSituationGroup : IdentifiedEntity
    {
        private const int DescriptionLength = 1024 * 500;

        public LifeSituationGroup()
        {
            SortId = DateTime.Now.Ticks;
        }

        [NotNullNotEmpty(Message = "Код группы услуг не указан")]
        [Property]
        public virtual string Code { get; set; }

        [Length(Message = "Название группы услуг не указано")]
        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Comment { get; set; }

        [Property(Length = DescriptionLength)]
        public virtual string Description { get; set; }

        [Range(Min = 1, Max = 10, Message = "Количество колонок должно быть от 1 до 10")]
        [Property]
        public virtual int Columns { get; set; }

        [Range(Min = 1, Max = 15, Message = "Количество строк должно быть от 1 до 10")]
        [Property]
        public virtual int Rows { get; set; }

        [Property]
        public virtual string Color { get; set; }

        [Property]
        public virtual float FontSize { get; set; }

        [Property]
        public virtual byte[] Icon { get; set; }

        [ManyToOne(ClassType = typeof(LifeSituationGroup), Column = "ParentGroupId", ForeignKey = "LifeSituationGroupToParentGroupReference")]
        public virtual LifeSituationGroup ParentGroup { get; set; }

        [Property]
        public virtual bool IsActive { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        public override string ToString()
        {
            return string.Format("{0} {1}", Code, Name);
        }
    }
}