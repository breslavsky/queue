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

        public ConfigurationManager(string app)
        {
            this.app = app;

            Load();
        }

        private void Load()
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                                    "Junte",
                                                                    Assembly.GetEntryAssembly().GetName().Name,
                                                                    "app.config");

            configuration = NetConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        public T GetSection<T>(string key, Action<T> initialize = null) where T : ConfigurationSection, new()
        {
            T section = configuration.GetSection(key) as T;

            if (section == null)
            {
                section = new T();
                if (initialize != null)
                {
                    initialize(section);
                }
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