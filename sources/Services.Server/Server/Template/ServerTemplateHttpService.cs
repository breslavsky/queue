using Queue.Services.Contracts;
using System.IO;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class ServerTemplateHttpService : ServerTemplateService, IServerTemplateHttpService
    {
        public Stream GetTemplate(string app, string theme, string template)
        {
            return base.ReadTemplate(app, theme, template);
        }
    }
}