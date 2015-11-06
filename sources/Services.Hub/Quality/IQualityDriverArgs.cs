namespace Queue.Services.Hub
{
    public interface IQualityDriverArgs
    {
        byte DeviceId { get; set; }

        byte Rating { get; set; }
    }
}