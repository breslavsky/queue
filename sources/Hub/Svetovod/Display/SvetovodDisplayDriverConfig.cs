using Queue.Hub.Settings;
using System.Configuration;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayDriverConfig : DriverConfig
    {
        [ConfigurationProperty("port")]
        public string Port
        {
            get { return (string)this["port"]; }
            set { this["port"] = value; }
        }

        [ConfigurationProperty("deviceId")]
        public byte DeviceId
        {
            get { return (byte)this["deviceId"]; }
            set { this["deviceId"] = value; }
        }

        [ConfigurationProperty("connections")]
        [ConfigurationCollection(typeof(SvetovodDisplayConnectionCollection))]
        public SvetovodDisplayConnectionCollection Connections
        {
            get { return (SvetovodDisplayConnectionCollection)base["connections"]; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}