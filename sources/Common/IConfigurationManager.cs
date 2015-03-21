using System.Configuration;

namespace Queue.Common
{
    public interface IConfigurationManager
    {
        T GetSection<T>(string key) where T : ConfigurationSection, new();

        void Save();
    }
}