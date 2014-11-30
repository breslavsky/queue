using System;
using System.Collections.Generic;
using System.Resources;

namespace Queue.Model.Common
{
    public class EnumItem<T>
    {
        public T Value { get; set; }

        public EnumItem(T value)
        {
            this.Value = value;
        }

        private const string TranslationAssemblyPattern = "Queue.Model.Common.Translation.{0}";

        public static EnumItem<T>[] GetItems()
        {
            var result = new List<EnumItem<T>>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                result.Add(new EnumItem<T>(value));
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