using System;
using System.Collections.Generic;

namespace Queue.Services.Hub
{
    public interface IHubQualityDriver : IHubDriver
    {
        event EventHandler<IHubQualityDriverArgs> Accepted;

        Dictionary<byte, int> Answers { get; private set; }

        void Enable(byte deviceId);

        void Disable(byte deviceId);
    }
}