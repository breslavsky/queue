using Junte.WCF.Common;
using log4net;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.IO;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    public partial class PortalService : IPortalService
    {
        private const string WEB_CLIENT_PATH = "\\projects\\queue\\trunk\\Portal";

        private static readonly ILog logger = LogManager.GetLogger(typeof(PortalClientService));

        private Guid sessionId;

        public PortalService(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser)
        {
            ChannelBuilder = channelBuilder;
            CurrentAdministrator = currentUser;

            Request = WebOperationContext.Current.IncomingRequest;
            try
            {
                sessionId = Guid.Parse(Request.Headers[ExtendHttpHeaders.SESSION]);
            }
            catch (Exception exception)
            {
                logger.Warn(exception);
            }

            Response = WebOperationContext.Current.OutgoingResponse;
        }

        protected DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        protected Administrator CurrentAdministrator { get; private set; }

        protected IncomingWebRequestContext Request { get; private set; }

        protected OutgoingWebResponseContext Response { get; private set; }

        public virtual Stream Index()
        {
            return GetContent("index.html");
        }

        public Stream Favicon()
        {
            return GetContent("favicon.ico");
        }

        public Stream ClientAccessPolicy()
        {
            return GetContent("clientaccesspolicy.xml");
        }

        public Stream GetContent(string path)
        {
            string file = Directory.Exists(WEB_CLIENT_PATH)
                ? string.Format("{0}\\www\\{1}", WEB_CLIENT_PATH, path)
                : file = string.Format("www\\{0}", path);

            Response.ContentType = ContentType.GetType(Path.GetExtension(file));
            return File.Open(file, FileMode.Open);
        }

        public async Task<DefaultConfig> GetDefaultConfig()
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    return await channel.Service.GetDefaultConfig();
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<PortalConfig> GetPortalConfig()
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    return await channel.Service.GetPortalConfig();
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<Service[]> FindServices(string filter)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    return await channel.Service.FindServices(filter, 0, 50);
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hour, int minute, int second)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    return await channel.Service.GetQueuePlanMetric(year, month, day, hour, minute, second);
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<QueuePlanServiceMetric> GetQueuePlanServiceMetric(int year, int month, int day, int hours, int minute, int second, string serviceId)
        {
            using (var channel = ChannelBuilder.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(CurrentAdministrator.SessionId);
                    return await channel.Service.GetQueuePlanServiceMetric(year, month, day, hours, minute, second, Guid.Parse(serviceId));
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        protected struct ExtendHttpHeaders
        {
            public const string LOCATION = "Location";
            public const string SESSION = "SessionId";
        }
    }
}