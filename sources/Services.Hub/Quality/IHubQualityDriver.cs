using System;

namespace Queue.Services.Hub
{
    public interface IHubQualityDriver : IHubDriver
    {
        event EventHandler<IHubQualityDriverArgs> Accepted;

        void Enable(byte deviceId);

        void Disable(byte deviceId);
    }
}