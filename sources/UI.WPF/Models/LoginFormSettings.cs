using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.UI.WPF.Models
{
    public class LoginFormSettings : ConfigurationSection
    {
        public const string SectionKey = "loginForm";

        public LoginFormSettings()
        {
            Language = CultureInfo.CurrentCulture.GetLanguage();
        }

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