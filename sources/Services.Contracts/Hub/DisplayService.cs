namespace Queue.Services.Contracts.Hub
{
    public class DisplayService : ClientService<IDisplayTcpService>
    {
        public DisplayService(string endpoint)
            : base(endpoint, ServicesPaths.Display)
        {
        }
    }
}