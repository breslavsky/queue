using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.UI.WPF
{
    public class LoginFormSettings : ConfigurationSection
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