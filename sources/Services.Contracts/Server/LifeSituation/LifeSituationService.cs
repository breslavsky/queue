namespace Queue.Services.Contracts.Server
{
    public class LifeSituationService : ClientService<ILifeSituationTcpService>
    {
        public LifeSituationService(string endpoint)
            : base(endpoint, ServicesPaths.LifeSituation)
        {
        }
    }
}