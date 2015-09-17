using System;
using System.Configuration;
using System.Reflection;
using System.Xml;

namespace Queue.Hub.Settings
{
    public class DriverElementConfig : ConfigurationElement
    {
        private DriverConfig config = null;

        public DriverConfig Config
        {
            get { return config; }
        }

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            string assembly = reader.GetAttribute("assembly");
            string settings = reader.GetAttribute("settings");
            config = Activator.CreateInstance(Assembly.Load(assembly).GetType(settings)) as DriverConfig;
            config.ConfigDeserializeElement(reader, serializeCollectionKey);
        }
    }
}