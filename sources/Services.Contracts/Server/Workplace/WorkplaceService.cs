namespace Queue.Services.Contracts.Server
{
    public class WorkplaceService : ClientService<IWorkplaceTcpService>
    {
        public WorkplaceService(string endpoint)
            : base(endpoint, ServicesPaths.Workplace)
        {
        }
    }
}