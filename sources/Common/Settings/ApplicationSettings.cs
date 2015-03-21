using System.Configuration;
using System.Threading;

namespace Queue.Common
{
    public class ApplicationSettings : ConfigurationSection
    {
        public ApplicationSettings()
        {
            Language = Thread.CurrentThread.CurrentCulture.GetLanguage();
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