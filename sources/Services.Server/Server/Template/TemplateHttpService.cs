using Queue.Services.Contracts.Server;
using System.IO;
using System.ServiceModel;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class TemplateHttpService : TemplateService, ITemplateHttpService
    {
        public Stream GetTemplate(string app, string theme, string template)
        {
            return base.ReadTemplate(app, theme, template);
        }
    }
}