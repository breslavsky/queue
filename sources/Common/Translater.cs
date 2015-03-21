using System;
using System.Linq;
using System.Reflection;
using System.Resources;

namespace Queue.Common
{
    public static class Translater
    {
        public static string Message(string key, params object[] parameters)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            try
            {
                string message = new ResourceManager(GetResourceBaseName(assembly, "Messages"), assembly).GetString(key);
                return string.Format(message, parameters);
            }
            catch
            {
                return key;
            }
        }

        public static string Enum<T>(T value, string resName = null) where T : struct, IConvertible
        {
            string result = null;

            Assembly assembly = typeof(T).Assembly;
            try
            {
                result = new ResourceManager(GetResourceBaseName(assembly, resName ?? typeof(T).Name), assembly)
                                    .GetString(value.ToString());
            }
            catch
            {
            }

            return result ?? value.ToString();
        }

        private static string GetResourceBaseName(Assembly assembly, string resource)
        {
            string ends = string.Format("Translation.{0}.resources", resource);

            string result = assembly.GetManifestResourceNames()
                                          .FirstOrDefault(n => n.EndsWith(ends));

            return result.Substring(0, result.LastIndexOf('.'));
        }
    }
}