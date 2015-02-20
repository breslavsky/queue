using System;
using System.Collections.Generic;
using System.Resources;

namespace Queue.Common
{
    public class EnumItem<T>
    {
        private const string TranslationPattern = "{0}.Translation.{1}";

        public T Value { get; set; }

        public EnumItem(T value)
        {
            this.Value = value;
        }

        public static EnumItem<T>[] GetItems()
        {
            List<EnumItem<T>> result = new List<EnumItem<T>>();
            foreach (T value in Enum.GetValues(typeof(T)))
            {
                result.Add(new EnumItem<T>(value));
            }

            return result.ToArray();
        }

        public override string ToString()
        {
            Type type = typeof(T);
            try
            {
                return new ResourceManager(string.Format(TranslationPattern, type.Namespace, type.Name), type.Assembly)
                    .GetString(Value.ToString());
            }
            catch
            {
                return Value.ToString();
            }
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