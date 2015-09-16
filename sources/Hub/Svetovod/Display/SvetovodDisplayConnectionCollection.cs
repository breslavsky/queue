using System;
using System.Configuration;

namespace Queue.Hub.Svetovod
{
    [ConfigurationCollection(typeof(SvetovodDisplayConnectionConfig))]
    public class SvetovodDisplayConnectionCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "connection";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        public void Add(SvetovodDisplayConnectionConfig element)
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
            return new SvetovodDisplayConnectionConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SvetovodDisplayConnectionConfig)(element)).Sysnum;
        }
    }
}