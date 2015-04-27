using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NetConfigurationManager = System.Configuration.ConfigurationManager;

namespace Queue.Common
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly string app;
        private Configuration configuration;
        private Environment.SpecialFolder folder;

        public ConfigurationManager(string app = null, Environment.SpecialFolder folder = Environment.SpecialFolder.ApplicationData)
        {
            this.app = app ?? Assembly.GetEntryAssembly().GetName().Name;
            this.folder = folder;

            Load();
        }

        private void Load()
        {
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename = Path.Combine(Environment.GetFolderPath(folder), "Junte", app, "app.config")
            };

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