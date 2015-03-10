using Queue.Common;
using System.Configuration;

namespace Queue.Media
{
    public class MediaSettings : AbstractSettings
    {
        public MediaSettings()
        {
            Port = 9090;
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("folder", IsRequired = true)]
        public string Folder
        {
            get { return (string)this["folder"]; }
            set { this["folder"] = value; }
        }
    }
}