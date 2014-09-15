using System;
using System.Collections.Generic;
using System.Resources;

namespace Queue.Model.Common
{
    public abstract class DataListItem
    {
        public const string Id = "Id";
        public const string Key = "Key";
        public const string Value = "Value";
    }

    public class EnumDataListItem : DataListItem
    {
        private const string TranslationAssemblyPattern = "Queue.Model.Common.Translation.{0}";

        public static List<KeyValuePair<T, string>> GetList<T>()
        {
            List<KeyValuePair<T, string>> result = new List<KeyValuePair<T, string>>();
            ResourceManager resourceManager = new ResourceManager(string.Format(TranslationAssemblyPattern, typeof(T).Name), typeof(T).Assembly);
            foreach (T key in Enum.GetValues(typeof(T)))
            {
                result.Add(new KeyValuePair<T, string>(key, resourceManager.GetString(key.ToString())));
            }

            return result;
        }
    }
}