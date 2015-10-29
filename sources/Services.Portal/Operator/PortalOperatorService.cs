using Junte.WCF;

using NLog;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System.IO;
using System.ServiceModel;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
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