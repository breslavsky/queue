using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Queue.Common
{
    public class Translater
    {
        private const string ResourcePathTemplate = "{Assembly}.Translate.{Name}";

        private readonly ResourceManager manager;

        private Translater(Assembly assembly, string resource, string modification = null)
        {
            this.manager = CreateResouceManager(assembly, resource, modification);
        }

        public Translater(Type type, string modification = null)
        {
            this.manager = CreateResouceManager(type.Assembly, type.Name, modification);
        }

        private ResourceManager CreateResouceManager(Assembly assembly, string name, string modification = null)
        {
            try
            {
                string path = string.Format("{0}.Translate.{1}{2}",
                                        assembly.GetName().Name,
                                        name,
                                        modification == null ? string.Empty : "_" + modification);

                return new ResourceManager(path, assembly);
            }
            catch { }

            return null;
        }

        public DictionaryEntry[] GetStrings()
        {
            DictionaryEntry[] result = new DictionaryEntry[] { };
            if (manager != null)
            {
                try
                {
                    result = manager.GetResourceSet(CultureInfo.CurrentCulture, true, true)
                                         .Cast<DictionaryEntry>()
                                         .ToArray();
                }
                catch { }
            }
            return result;
        }

        public string GetString(string key, params object[] parameters)
        {
            string result = null;

            if (manager != null)
            {
                try
                {
                    string str = manager.GetString(key);
                    result = parameters.Length > 0 ? string.Format(str, parameters) : str;
                }
                catch { }
            }

            return result ?? key;
        }

        public static string Message(string key, params object[] parameters)
        {
            return new Translater(Assembly.GetCallingAssembly(), "Messages").GetString(key, parameters);
        }

        public static string Enum<T>(T value, string mod = null) where T : struct, IConvertible
        {
            return new Translater(typeof(T)).GetString(value.ToString());
        }
    }
}