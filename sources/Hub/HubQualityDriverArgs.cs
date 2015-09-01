using Queue.Services.Hub.Quality;

namespace Queue.Hub
{
    public class HubQualityDriverArgs : IHubQualityDriverArgs
    {
        public int Rating { get; set; }
    }
}