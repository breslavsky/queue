using Queue.Common;
using System.Configuration;

namespace Queue.Portal
{
    public class PortalSettings : ConfigurationSection
    {
        public const string SectionKey = "portal";

        public PortalSettings()
        {
            Port = 9090;
            Theme = Templates.Themes.Default;
        }

        [ConfigurationProperty("port")]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("theme")]
        public string Theme
        {
            get { return (string)this["theme"]; }
            set { this["theme"] = value; }
        }
    }
}