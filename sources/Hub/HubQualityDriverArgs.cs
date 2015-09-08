using Queue.Services.Hub;

namespace Queue.Hub
{
    public class HubQualityDriverArgs : IHubQualityDriverArgs
    {
        public byte DeviceId { get; set; }
        public byte Rating { get; set; }
    }
}