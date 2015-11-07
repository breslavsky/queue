using Queue.Common;
using System.Configuration;

namespace Queue.Notification
{
    public class AppSettings : ConfigurationSection
    {
        public const string SectionKey = "notificationSettings";

        public AppSettings()
        {
            Theme = "default";
            Endpoint = "net.tcp://queue:4505";
        }

        [ConfigurationProperty("endpoint")]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("theme")]
        public string Theme
        {
            get { return (string)this["theme"]; }
            set { this["theme"] = value; }
        }

        [ConfigurationProperty("isRemember")]
        public bool IsRemember
        {
            get { return (bool)this["isRemember"]; }
            set { this["isRemember"] = value; }
        }

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        [ConfigurationProperty("accent")]
        public string Accent
        {
            get { return (string)this["accent"]; }
            set { this["accent"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}