using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_design", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "DesignConfigToConfigReference")]
    public class DesignConfig : Config
    {
        public DesignConfig()
        {
            Type = ConfigType.Design;
        }

        #region properties

        [Property]
        public virtual byte[] LogoSmall { get; set; }

        #endregion properties
    }
}