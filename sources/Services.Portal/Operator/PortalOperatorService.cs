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
    public class PortalOperatorService : PortalService, IPortalOperatorService
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(PortalOperatorService));

        private Guid sessionId;
        private Operator currentOperator;

        public PortalOperatorService(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser)
            : base(channelBuilder, currentUser)
        {
            try
            {
                sessionId = Guid.Parse(Request.Headers[ExtendHttpHeaders.SESSION]);

                using (var channel = ChannelBuilder.CreateChannel())
                {
                    try
                    {
                        currentOperator = (Operator)channel.Service.OpenUserSession(sessionId).Result;
                    }
                    catch (FaultException exception)
                    {
                        throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.Warn(exception);
            }
        }

        public override Stream Index()
        {
            return GetContent("operator\\index.html");
        }
    }
}