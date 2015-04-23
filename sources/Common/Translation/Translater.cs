using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Queue.Common
{
    public class Translater : ITranslater
    {
        private readonly ResourceManager manager;

        private Translater(Assembly assembly, string resource, string modification = null)
        {
            manager = CreateResouceManager(assembly, resource, modification);
        }

        public Translater(Type type, string modification = null)
        {
            manager = CreateResouceManager(type.Assembly, type.Name, modification);
        }

        private ResourceManager CreateResouceManager(Assembly assembly, string name, string modification = null)
        {
            try
            {
                var path = string.Format("{0}.Translate.{1}{2}",
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
            DictionaryEntry[] result = { };
            if (manager == null)
            {
                return result;
            }

            try
            {
                return manager.GetResourceSet(CultureInfo.CurrentCulture, true, true)
                    .Cast<DictionaryEntry>()
                    .ToArray();
            }
            catch
            {
                return result;
            }
        }

        public string GetString(object key, params object[] parameters)
        {
            string result = null;

            if (manager != null)
            {
                try
                {
                    var str = manager.GetString(key.ToString());
                    result = parameters.Length > 0 ? string.Format(str, parameters) : str;
                }
                catch { }
            }

            return result ?? key.ToString();
        }

        public static string Message(string key, params object[] parameters)
        {
            return new Translater(Assembly.GetCallingAssembly(), "Messages").GetString(key, parameters);
        }

        public static string Enum<T>(T value, string mod = null) where T : struct, IConvertible
        {
            ITranslater translater = null;

            if (value is DayOfWeek)
            {
                translater = new DayOfWeekTranslater();
            }
            else
            {
                translater = new Translater(typeof(T), mod);
            }

            return translater.GetString(value);
        }
    }
}