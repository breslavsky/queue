using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NetConfigurationManager = System.Configuration.ConfigurationManager;

namespace Queue.Common
{
    public class ConfigurationManager : IConfigurationManager
    {
        private string app;
        private Configuration configuration;

        public ConfigurationManager(string app = null)
        {
            this.app = app ?? Assembly.GetEntryAssembly().GetName().Name;

            Load();
        }

        private void Load()
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();

            configMap.ExeConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                                    "Junte",
                                                                     app,
                                                                    "app.config");

            configuration = NetConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        public T GetSection<T>(string key) where T : ConfigurationSection, new()
        {
            T section = null;
            try
            {
                section = configuration.Sections[key] as T;
            }
            catch
            {
                configuration.Sections.Remove(key);
            }

            if (section == null)
            {
                section = new T();
                configuration.Sections.Add(key, section);
            }

            return section;
        }

        public void Save()
        {
            configuration.Save();
        }
    }
}