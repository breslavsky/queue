using Queue.Common;
using System.Configuration;

namespace Queue.Notification.Settings
{
    public class AppSettings : ConfigurationSection
    {
        public const string SectionKey = "notification";

        public AppSettings()
        {
            Theme = "default";
            Endpoint = "net.tcp://queue:4505";
            IsFullScreen = true;
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

        [ConfigurationProperty("isFullScreen", DefaultValue = true)]
        public bool IsFullScreen
        {
            get { return (bool)this["isFullScreen"]; }
            set { this["isFullScreen"] = value; }
        }

        [ConfigurationProperty("screenNumber")]
        public byte ScreenNumber
        {
            get { return (byte)this["screenNumber"]; }
            set { this["screenNumber"] = value; }
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

        [ConfigurationProperty("displays")]
        [ConfigurationCollection(typeof(DisplayCollection))]
        public DisplayCollection Displays
        {
            get { return (DisplayCollection)base["displays"]; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}