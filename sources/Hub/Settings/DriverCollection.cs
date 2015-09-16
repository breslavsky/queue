using System;
using System.Configuration;

namespace Queue.Hub.Settings
{
    [ConfigurationCollection(typeof(DriverElementConfig))]
    public class DriverCollection : ConfigurationElementCollection
    {
        internal const string PropertyName = "driver";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        public void Add(DriverElementConfig element)
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
            return new DriverElementConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DriverElementConfig)(element)).Config.Name;
        }

        public DriverConfig this[int idx]
        {
            get { return ((DriverElementConfig)BaseGet(idx)).Config; }
        }
    }
}