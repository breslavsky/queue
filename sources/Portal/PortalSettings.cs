using Queue.Common;
using System.Configuration;

namespace Queue.Portal
{
    public class PortalSettings : AbstractSettings
    {
        public PortalSettings()
        {
            Port = 9090;
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }
    }
}