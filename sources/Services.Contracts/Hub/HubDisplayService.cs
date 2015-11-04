namespace Queue.Services.Contracts
{
    public class HubDisplayService : ClientService<IHubDisplayTcpService>
    {
        public HubDisplayService(string endpoint)
            : base(endpoint, HubServicesPaths.Display)
        {
        }
    }
}