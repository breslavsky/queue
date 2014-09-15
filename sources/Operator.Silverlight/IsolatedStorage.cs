using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Queue.Operator.Silverlight
{
    public static class IsolatedStorage<T>
    {
        public static void Put(T obj)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            string filename = typeof(T).ToString();
            using (IsolatedStorageFileStream stream = storage.OpenFile(filename, FileMode.Create))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(stream, obj);
                stream.Close();
            }
        }
        public static T Get()
        {
            T obj = default(T);
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            string filename = typeof(T).ToString();
            if (storage.FileExists(filename))
            {
                using (IsolatedStorageFileStream stream = storage.OpenFile(filename, FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    obj = (T)serializer.ReadObject(stream);
                }
            }
            return obj;
        }
    }
}
