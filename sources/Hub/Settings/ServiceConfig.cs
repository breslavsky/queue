using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Hub.Settings
{
    public class ServiceConfig : ConfigurationElement
    {
        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("host")]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }

        [ConfigurationProperty("port")]
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