namespace Queue.Common
{
    public interface IConfigurationManager
    {
        T GetSection<T>(string key) where T : AbstractSettings, new();

        void Save();
    }
}