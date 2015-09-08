namespace Queue.Services.Hub
{
    public interface IHubQualityDriverArgs
    {
        byte DeviceId { get; set; }
        int Rating { get; set; }
    }
}