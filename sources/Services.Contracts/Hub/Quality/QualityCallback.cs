using Queue.Services.DTO;
using System;

namespace Queue.Services.Contracts.Hub
{
    public class QualityCallback : IQualityCallback
    {
        public event EventHandler<QualityEventArgs> OnAccepted = delegate { };

        public void Accepted(byte rating)
        {
            OnAccepted(this, new QualityEventArgs() { Rating = rating });
        }
    }
}