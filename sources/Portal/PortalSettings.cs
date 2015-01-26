using System.Configuration;

namespace Queue.Portal
{
    public class PortalSettings : ConfigurationSection
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }
    }
}