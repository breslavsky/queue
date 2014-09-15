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
}