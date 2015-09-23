namespace Queue.Services.Contracts
{
    public class HubDisplayService : ClientService<IHubDisplayTcpService>
    {
        public HubDisplayService(string endpoint, string path)
            : base(endpoint, path)
        {
        }
    }
}