using Queue.Common;
using System.Configuration;

namespace Queue.Terminal
{
    public class AppSettings : ConfigurationSection
    {
        public const string SectionKey = "loginForm";

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