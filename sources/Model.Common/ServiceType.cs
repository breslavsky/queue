using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ServiceType
    {
        None = 0,
        Consultation = 1,
        ReceiptOfDocuments = 2,
        ReleaseOfDocuments = 4
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this ServiceType value)
        {
            return Translation.ServiceType.ResourceManager.GetString(value.ToString());
        }
    }
}