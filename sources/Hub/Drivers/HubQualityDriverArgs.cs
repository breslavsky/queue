using Queue.Services.Hub;

namespace Queue.Hub
{
    public class HubQualityDriverArgs : IQualityDriverArgs
    {
        public byte DeviceId { get; set; }

        public byte Rating { get; set; }
    }
}