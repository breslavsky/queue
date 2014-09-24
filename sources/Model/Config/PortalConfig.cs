using Junte.Data.Common;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_portal", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "PortalConfigToConfigReference")]
    public class PortalConfig : Config
    {
        public PortalConfig()
        {
            Type = ConfigType.Portal;
        }

        #region properties

        [Property(Length = DataLength._500K)]
        public virtual string Header { get; set; }

        [Property(Length = DataLength._500K)]
        public virtual string Footer { get; set; }

        [Property]
        public virtual bool CurrentDayRecording { get; set; }

        #endregion properties
    }
}