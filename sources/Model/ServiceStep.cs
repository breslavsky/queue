using Junte.Data.Common;
using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "service_step", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class ServiceStep : IdentifiedEntity
    {
        public ServiceStep()
        {
            Name = "Новый этап";
            SortId = DateTime.Now.Ticks;
        }

        [Length(1, 255, Message = "Поле (наименование этапа) должно быть больше 1 и менее 255 символов")]
        [Property]
        public virtual string Name { get; set; }

        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ServiceStepToServiceReference")]
        public virtual Service Service { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}