using Junte.Configuration;
using System.ComponentModel;
using System.Configuration;

namespace Queue.Notification.Settings
{
    public class WorkplaceConfig : ConfigurationElement
    {
        [ConfigurationProperty("number")]
        public byte Number
        {
            get { return (byte)this["number"]; }
            set { this["number"] = value; }
        }
    }
}