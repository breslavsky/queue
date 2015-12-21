using System;
using System.Configuration;

namespace Queue.Notification.Settings
{
    [ConfigurationCollection(typeof(WorkplaceConfig))]
    public class WorkplaceCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "workplace";

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMapAlternate; }
        }

        public void Add(WorkplaceConfig element)
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
            return new WorkplaceConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((WorkplaceConfig)(element)).Number;
        }
    }
}