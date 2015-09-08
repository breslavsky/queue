namespace Queue.Services.Hub
{
    public interface IHubQualityDriverArgs
    {
        byte DeviceId { get; set; }
        byte Rating { get; set; }
    }
}