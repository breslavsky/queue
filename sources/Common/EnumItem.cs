﻿using System;
using System.Collections.Generic;

namespace Queue.Common
{
    public class EnumItem<T> where T : struct, IConvertible
    {
        private const string TranslationPattern = "{0}.Translate.{1}";

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