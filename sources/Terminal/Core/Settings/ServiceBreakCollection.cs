using System;
using System.Configuration;

namespace Queue.Terminal.Core.Settings
{
    [ConfigurationCollection(typeof(ServiceBreak))]
    public class ServiceBreakCollection : ConfigurationElementCollection
    {
        private const string PropertyName = "serviceBreak";

        public ServiceBreak this[int index]
        {
            get { return base.BaseGet(index) as ServiceBreak; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        public void Add(ServiceBreak element)
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
            return new ServiceBreak();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceBreak)(element)).From;
        }
    }
}