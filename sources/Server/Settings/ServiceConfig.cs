using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Server.Settings
{
    public class ServiceConfig : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}