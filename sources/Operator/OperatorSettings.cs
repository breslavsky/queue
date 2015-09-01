using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.Operator
{
    public class OperatorSettings : ConfigurationSection
    {
        public const string SectionKey = "operator";

        [ConfigurationProperty("hubQuality")]
        public HubQualityConfig HubQuality
        {
            get { return (HubQualityConfig)this["hubQuality"]; }
            set { this["hubQuality"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class HubQualityConfig : ConfigurationElement
    {
        [ConfigurationProperty("endpoint", IsRequired = true)]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}