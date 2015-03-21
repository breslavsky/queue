using Queue.Common;
using System.Configuration;

namespace Queue.UI.WPF.Models
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

        [ConfigurationProperty("accent")]
        public string Accent
        {
            get { return (string)this["accent"]; }
            set { this["accent"] = value; }
        }

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}