using System;

namespace Queue.Services.Hub
{
    public interface IHubDisplayDriver : IHubDriver
    {
        void ShowNumber(byte deviceId, short number);
    }
}