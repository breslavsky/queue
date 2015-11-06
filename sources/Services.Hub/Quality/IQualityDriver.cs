using System;
using System.Collections.Generic;

namespace Queue.Services.Hub
{
    public interface IQualityDriver : IHubDriver
    {
        event EventHandler<IQualityDriverArgs> Accepted;

        Dictionary<byte, byte> Answers { get; set; }

        void Enable(byte deviceId);

        void Disable(byte deviceId);
    }
}