using Junte.WCF.Common;
using log4net;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    public sealed class PortalOperatorService : PortalService, IPortalOperatorService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PortalOperatorService));

        public PortalOperatorService(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser)
            : base(channelBuilder, currentUser)
        {
        }

        public override Stream Index()
        {
            return GetContent("operator\\index.html");
        }
    }
}