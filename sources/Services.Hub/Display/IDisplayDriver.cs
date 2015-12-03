namespace Queue.Services.Hub
{
    public interface IDisplayDriver : IHubDriver
    {
        void ShowText(byte deviceId, string text);

        void ClearText(byte deviceId);

        void ShowLines(byte deviceId, ushort[][] lines);
    }
}