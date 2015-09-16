namespace Queue.Services.Hub
{
    public interface IHubDisplayDriver : IHubDriver
    {
        void ShowText(byte deviceId, string text);
    }
}