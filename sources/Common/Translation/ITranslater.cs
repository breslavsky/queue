namespace Queue.Common
{
    internal interface ITranslater
    {
        string GetString(object key, params object[] parameters);
    }
}