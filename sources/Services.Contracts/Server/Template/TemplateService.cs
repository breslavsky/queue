namespace Queue.Services.Contracts.Server
{
    public class TemplateService : ClientService<ITemplateTcpService>
    {
        public TemplateService(string endpoint)
            : base(endpoint, ServicesPaths.Template)
        {
        }
    }
}