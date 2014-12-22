using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using System;

namespace Queue.Model
{
    [Class(Table = "office", DynamicUpdate = true, Lazy = false)]
    public class Office : IdentifiedEntity
    {
        #region properties

        [NotNull(Message = "Название филиала не указано")]
        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual string Endpoint { get; set; }

        [Property]
        public virtual Guid SessionId { get; set; }

        #endregion properties

        public override string ToString()
        {
            return Name;
        }
    }
}