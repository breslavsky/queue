using NLog;
using Queue.Services.Contracts.Portal;
using System.IO;
using System.ServiceModel;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true)]
    public sealed class PortalOperatorService : PortalService, IPortalOperatorService
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public PortalOperatorService()
            : base()
        {
        }

        public override Stream Index()
        {
            return GetContent("operator\\index.html");
        }
    }
}