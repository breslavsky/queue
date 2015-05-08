using Junte.Translation;
using System;
using System.Collections.Generic;

namespace Queue.Common
{
    public class EnumItem<T> where T : struct, IConvertible
    {
        public T Value { get; set; }

        public EnumItem(T value)
        {
            Value = value;
        }

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
            return Translater.Enum(Value);
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