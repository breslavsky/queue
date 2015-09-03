using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Hub.Settings
{
    public class DriversConfig : ConfigurationElement
    {
        [ConfigurationProperty("quality")]
        public DriverCollection Quality
        {
            get { return (DriverCollection)this["quality"]; }
            set { this["quality"] = value; }
        }

        [ConfigurationProperty("display")]
        public DriverCollection Display
        {
            get { return (DriverCollection)this["display"]; }
            set { this["display"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}