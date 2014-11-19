using System;
using System.Collections.Generic;
using System.Resources;

namespace Queue.Model.Common
{
    //TODO: remove!
    public abstract class DataListItem
    {
        public const string Id = "Id";
        public const string Text = "Text";
        public const string Key = "Key";
        public const string Value = "Value";
    }

    public class EnumDataListItem<T> : DataListItem
    {
        public T Value { get; set; }

        public EnumDataListItem(T value)
        {
            this.Value = value;
        }

        private const string TranslationAssemblyPattern = "Queue.Model.Common.Translation.{0}";

        public static EnumDataListItem<T>[] GetList()
        {
            var result = new List<EnumDataListItem<T>>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                result.Add(new EnumDataListItem<T>(value));
            }

            return result.ToArray();
        }

        public override string ToString()
        {
            var resourceManager = new ResourceManager(string.Format(TranslationAssemblyPattern, typeof(T).Name), typeof(T).Assembly);
            return resourceManager.GetString(Value.ToString());
        }

        public override bool Equals(object obj)
        {
            return GetHashCode() == obj.GetHashCode();
        }

        public override int GetHashCode()
        {
            return Value.ToString().GetHashCode();
        }
    }
}