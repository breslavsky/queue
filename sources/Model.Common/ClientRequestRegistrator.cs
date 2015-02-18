using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ClientRequestRegistrator : long
    {
        Terminal = 1,
        Manager = 2,
        Portal = 4
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this ClientRequestRegistrator value)
        {
            return Translation.ClientRequestRegistrator.ResourceManager.GetString(value.ToString());
        }
    }
}