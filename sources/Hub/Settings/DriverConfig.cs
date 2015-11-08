using System.Configuration;
using System.Xml;

namespace Queue.Hub.Settings
{
    public class DriverConfig : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true)]
        public string Name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }

        [ConfigurationProperty("enabled")]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("assembly")]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }

        [ConfigurationProperty("settings")]
        public string Settings
        {
            get { return (string)this["settings"]; }
            set { this["settings"] = value; }
        }

        [ConfigurationProperty("type")]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        public void ConfigDeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            DeserializeElement(reader, serializeCollectionKey);
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}