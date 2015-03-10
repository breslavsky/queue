using System.Configuration;

namespace Queue.Common
{
    public class ApplicationSettings : AbstractSettings
    {
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