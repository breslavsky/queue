using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.Common.Settings
{
    public class HubQualitySettings : ConfigurationSection
    {
        public const string SectionKey = "hubQuality";

        public HubQualitySettings()
        {
            Endpoint = "net.tcp://localhost:4511/";
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}