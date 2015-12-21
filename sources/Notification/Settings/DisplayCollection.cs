using System;
using System.Configuration;

namespace Queue.Notification.Settings
{
    [ConfigurationCollection(typeof(DisplayConfig))]
    public class DisplayCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "display";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        public void Add(DisplayConfig element)
        {
            LockItem = false;
            BaseAdd(element);
        }

        protected override string ElementName
        {
            get { return PropertyName; }
        }

        protected override bool IsElementName(string elementName)
        {
            return elementName.Equals(PropertyName, StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsReadOnly()
        {
            return false;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new DisplayConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DisplayConfig)(element)).DeviceId;
        }
    }
}