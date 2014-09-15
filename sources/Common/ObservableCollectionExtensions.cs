using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Queue.Common
{
    public static class ObservableCollectionUtils
    {
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> enumerable)
        {
            foreach (T i in enumerable)
            {
                collection.Add(i);
            }
        }

        public static int RemoveAll<T>(this ObservableCollection<T> list, Predicate<T> predicate)
        {
            int count = 0;

            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                    count++;
                }
            }

            return count;
        }
    }
}