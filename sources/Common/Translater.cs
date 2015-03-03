using System.Linq;
using System.Reflection;
using System.Resources;

namespace Queue.Common
{
    public static class Translater
    {
        private const string TranslationPattern = "{0}.Translation.Messages";

        public static string Message(string key, params object[] parameters)
        {
            Assembly assembly = Assembly.GetCallingAssembly();

            try
            {
                string messagesResources = assembly.GetManifestResourceNames()
                                             .FirstOrDefault(n => n.EndsWith("Translation.Messages.resources"));

                messagesResources = messagesResources.Substring(0, messagesResources.Length - 10);

                string message = new ResourceManager(messagesResources, assembly).GetString(key);
                return string.Format(message, parameters);
            }
            catch
            {
                return key;
            }
        }
    }
}