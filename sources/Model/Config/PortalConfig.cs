using Junte.Data.Common;
using NHibernate.Mapping.Attributes;
using Queue.Model.Common;

namespace Queue.Model
{
    [JoinedSubclass(Table = "config_portal", ExtendsType = typeof(Config), Lazy = false, DynamicUpdate = true)]
    [Key(Column = "ConfigId", ForeignKey = "PortalConfigToConfigReference")]
    public class PortalConfig : Config
    {
        private const int HeaderLength = 1024 * 500;
        private const int FooterLength = 1024 * 500;

        public PortalConfig()
        {
            Type = ConfigType.Portal;
        }

        #region properties

        [Property(Length = HeaderLength)]
        public virtual string Header { get; set; }

        [Property(Length = FooterLength)]
        public virtual string Footer { get; set; }

        [Property]
        public virtual bool CurrentDayRecording { get; set; }

        #endregion properties
    }
}