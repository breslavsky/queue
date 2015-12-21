using Junte.Configuration;
using System.ComponentModel;
using System.Configuration;

namespace Queue.Notification.Settings
{
    public class DisplayConfig : ConfigurationElement
    {
        [ConfigurationProperty("deviceId")]
        public byte DeviceId
        {
            get { return (byte)this["deviceId"]; }
            set { this["deviceId"] = value; }
        }

        [ConfigurationProperty("workplaces")]
        [ConfigurationCollection(typeof(WorkplaceCollection))]
        public WorkplaceCollection Workplaces
        {
            get { return (WorkplaceCollection)base["workplaces"]; }
        }
    }
}