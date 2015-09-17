using System.Configuration;
using System.Globalization;

namespace Queue.Common.Settings
{
    public class ApplicationSettings : ConfigurationSection
    {
        public const string SectionKey = "application";

        public ApplicationSettings()
        {
            Language = CultureInfo.CurrentCulture.GetLanguage();
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