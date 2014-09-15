using Junte.Data.Common;
using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "service_group", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class ServiceGroup : IdentifiedEntity
    {
        public ServiceGroup()
        {
            Code = "0.0";
            Name = "Новая группа услуг";
            Icon = new byte[] { };
            Columns = 2;
            Rows = 5;
            SortId = DateTime.Now.Ticks;
        }

        [Length(1, 15, Message = "Поле (код группы услуг) должно быть больше 1 и менее 15 символов")]
        [Property]
        public virtual string Code { get; set; }

        [Length(1, 255, Message = "Поле (название группы услуг) должно быть больше 1 и менее 255 символов")]
        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Comment { get; set; }

        [Property(Length = DataLength._500K)]
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
        public virtual byte[] Icon { get; set; }

        [ManyToOne(ClassType = typeof(ServiceGroup), Column = "ParentGroupId", ForeignKey = "ServiceGroupToParentGroupReference")]
        public virtual ServiceGroup ParentGroup { get; set; }

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