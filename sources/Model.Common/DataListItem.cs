using System;
using System.Collections.Generic;
using System.Resources;

namespace Queue.Model.Common
{
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

        public static List<KeyValuePair<T, string>> GetList()
        {
            List<KeyValuePair<T, string>> result = new List<KeyValuePair<T, string>>();
            ResourceManager resourceManager = new ResourceManager(string.Format(TranslationAssemblyPattern, typeof(T).Name), typeof(T).Assembly);
            foreach (T key in Enum.GetValues(typeof(T)))
            {
                result.Add(new KeyValuePair<T, string>(key, resourceManager.GetString(key.ToString())));
            }

            return result;
        }

        public override string ToString()
        {
            ResourceManager resourceManager = new ResourceManager(string.Format(TranslationAssemblyPattern, typeof(T).Name), typeof(T).Assembly);
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