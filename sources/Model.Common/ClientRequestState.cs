namespace Queue.Model.Common
{
    public enum ClientRequestState
    {
        Waiting,
        Calling,
        Absence,
        Rendering,
        Postponed,
        Rendered,
        Canceled
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this ClientRequestState value)
        {
            return Translation.ClientRequestState.ResourceManager.GetString(value.ToString());
        }
    }
}