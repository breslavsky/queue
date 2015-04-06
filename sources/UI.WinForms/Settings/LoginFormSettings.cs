using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.UI.WinForms
{
    public class LoginFormSettings : ConfigurationSection
    {
        public const string SectionKey = "login";

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        [ConfigurationProperty("isRemember")]
        public bool IsRemember
        {
            get { return (bool)this["isRemember"]; }
            set { this["isRemember"] = value; }
        }

        public LoginFormSettings()
        {
            Language = CultureInfo.CurrentCulture.GetLanguage();
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}