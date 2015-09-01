using System;

namespace Queue.Services.Hub.Quality
{
    public class HubQualityDriverArgs
    {
        public int Rating;
    }

    public interface IHubQualityDriver : IHubDriver
    {
        event EventHandler<HubQualityDriverArgs> Accepted;

        void Enable(int deviceId);

        void Disable(int deviceId);
    }
}