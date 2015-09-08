using System;
using System.Collections.Generic;

namespace Queue.Services.Hub
{
    public interface IHubQualityDriver : IHubDriver
    {
        event EventHandler<IHubQualityDriverArgs> Accepted;

        Dictionary<byte, byte> Answers { get; set; }

        void Enable(byte deviceId);

        void Disable(byte deviceId);
    }
}