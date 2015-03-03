namespace Queue.Common
{
    public interface ITranslater
    {
        string Message(string key, params object[] parameters);
    }
}