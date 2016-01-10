using System.Configuration;

namespace Queue.Notification.Settings
{
    public class WorkplaceConfig : ConfigurationElement
    {
        [ConfigurationProperty("number")]
        public int Number
        {
            get { return (int)this["number"]; }
            set { this["number"] = value; }
        }
    }
}