namespace Queue.Services.Contracts
{
    public class ServerTemplateService : ClientService<IServerTemplateTcpService>
    {
        public ServerTemplateService(string endpoint)
            : base(endpoint, ServerServicesPaths.Template)
        {
        }
    }
}