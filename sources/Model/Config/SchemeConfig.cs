using Junte.Data.Common;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_scheme", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "SchemeConfigToConfigReference")]
    public class SchemeConfig : Config
    {
        public SchemeConfig()
        {
            Type = ConfigType.Scheme;
        }

        #region properties

        [Property]
        public virtual int Version { get; set; }

        #endregion properties
    }
}