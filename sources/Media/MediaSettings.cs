using System.Configuration;

namespace Queue.Media
{
    public class MediaSettings : ConfigurationSection
    {
        public const string SectionKey = "media";

        public MediaSettings()
        {
            Port = 4507;
        }

        [ConfigurationProperty("port")]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }
    }
}