using System;
using System.Configuration;

namespace Queue.Hub.Svetovod
{
    [ConfigurationCollection(typeof(SvetovodDisplayConnectionColumnConfig))]
    public class SvetovodDisplayConnectionColumnCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "column";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        public void Add(SvetovodDisplayConnectionColumnConfig element)
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
            return new SvetovodDisplayConnectionColumnConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((SvetovodDisplayConnectionColumnConfig)(element)).Index;
        }
    }
}