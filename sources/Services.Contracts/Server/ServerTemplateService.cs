namespace Queue.Services.Contracts
{
    public class ServerTemplateService : ClientService<IServerTemplateTcpService>
    {
        public ServerTemplateService(string endpoint, string path)
            : base(endpoint, path)
        {
        }
    }
}