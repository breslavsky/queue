using Queue.Services.DTO;
using System;

namespace Queue.Services.Common
{
    public class HubQualityCallback : ICallback, IHubQualityCallback
    {
        public event EventHandler<HubQualityEventArgs> OnAccepted = delegate { };

        public void Accepted(byte rating)
        {
            OnAccepted(this, new HubQualityEventArgs() { Rating = rating });
        }
    }
}