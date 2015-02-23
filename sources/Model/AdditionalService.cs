using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;

namespace Queue.Model
{
    [Class(Table = "additional_service", DynamicUpdate = true, Lazy = false)]
    public class AdditionalService : IdentifiedEntity
    {
        #region properties

        [NotNull(Message = "Название дополнительной услуги не указано")]
        [Property]
        public virtual string Name { get; set; }

        [Property]
        public virtual decimal Price { get; set; }

        [Property]
        public virtual string Measure { get; set; }

        #endregion properties

        public override string ToString()
        {
            return Name;
        }
    }
}