using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [Class(Table = "config", Abstract = true, Lazy = false, DynamicUpdate = true)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public abstract class Config : Entity
    {
        #region properties

        //TODO: rename column Section -> Type
        [Id(Column = "Section", TypeType = typeof(ConfigType), Name = "Type")]
        public virtual ConfigType Type { get; set; }

        #endregion properties

        public override bool Equals(object obj)
        {
            Config target = obj as Config;
            return target != null ? target.GetHashCode() == GetHashCode() : false;
        }

        public override int GetHashCode()
        {
            return Type.GetHashCode();
        }
    }
}