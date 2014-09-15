using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Model.Common;
using System;

namespace Queue.Model
{
    [Class(Table = "service_parameter", Abstract = true, DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public abstract class ServiceParameter : IdentifiedEntity
    {
        // TODO: см. выше...
        public ServiceParameter()
        {
            Name = "Новый параметр";
        }

        #region properties

        [NotNull(Message = "Для параметра не указана услуга")]
        [ManyToOne(ClassType = typeof(Service), Column = "ServiceId", ForeignKey = "ServiceParameterToServiceReference")]
        public virtual Service Service { get; set; }

        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string ToolTip { get; set; }

        [Discriminator(Force = true, Column = "Type")]
        public virtual ServiceParameterType Type { get; set; }

        [Property]
        public virtual bool IsRequire { get; set; }

        [Property]
        public virtual long SortId { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("{0} {1}", Type, Name);
        }

        public virtual ClientRequestParameter Compile(object value)
        {
            throw new NotImplementedException();
        }
    }
}