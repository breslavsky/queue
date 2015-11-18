using Queue.Common;
using Queue.Terminal.Core.Settings;
using System;
using System.Configuration;

namespace Queue.Terminal
{
    public class AppSettings : ConfigurationSection
    {
        public const string SectionKey = "terminalSettings";

        public AppSettings()
        {
            Theme = "default";
            Endpoint = "net.tcp://queue:4505";
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

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("user")]
        public Guid User
        {
            get { return (Guid)this["user"]; }
            set { this["user"] = value; }
        }

        [ConfigurationProperty("endpoint")]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("serviceBreaks")]
        public ServiceBreakCollection ServiceBreaks
        {
            get { return this["serviceBreaks"] as ServiceBreakCollection; }
            set { this["serviceBreaks"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}