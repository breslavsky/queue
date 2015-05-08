using Junte.Data.NHibernate;
using Junte.Translation;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Common;
using Queue.Model.Common;

namespace Queue.Model
{
    [Class(Table = "workplace", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class Workplace : IdentifiedEntity
    {
        #region properties

        [Property]
        public virtual WorkplaceType Type { get; set; }

        [Property]
        [Min(Value = 0, Message = "Номер рабочего места должен быть больше 0")]
        public virtual int Number { get; set; }

        [Property]
        public virtual WorkplaceModificator Modificator { get; set; }

        [Property(Length = 1000)]
        public virtual string Comment { get; set; }

        [Property]
        public virtual byte Display { get; set; }

        [Property]
        public virtual byte Segments { get; set; }

        #endregion properties

        public override string ToString()
        {
            return string.Format("{0} {1}{2}", Translater.Enum(Type), Number, Translater.Enum(Modificator));
        }
    }
}