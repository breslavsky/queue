using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ClientRequestType
    {
        Live,
        Early
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this ClientRequestType value)
        {
            return Translation.ClientRequestType.ResourceManager.GetString(value.ToString());
        }
    }
}