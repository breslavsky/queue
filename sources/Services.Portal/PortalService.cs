using Junte.WCF.Common;
using NLog;
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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple,
        IncludeExceptionDetailInFaults = true, UseSynchronizationContext = false)]
    public partial class PortalService : IPortalService
    {
        private const string DebugWebClientPath = "\\git\\queue\\sources\\Portal\\www";
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly DuplexChannelBuilder<IServerTcpService> channelBuilder;
        protected readonly ChannelManager<IServerTcpService> channelManager;
        private readonly User currentUser;
        protected readonly IncomingWebRequestContext request;
        protected readonly OutgoingWebResponseContext response;

        public PortalService(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);

            request = WebOperationContext.Current.IncomingRequest;
            response = WebOperationContext.Current.OutgoingResponse;
        }

        public Stream ClientAccessPolicy()
        {
            return GetContent("clientaccesspolicy.xml");
        }

        public Stream Favicon()
        {
            return GetContent("favicon.ico");
        }

        public async Task<Service[]> FindServices(string query)
        {
            using (var channel = channelBuilder.CreateChannel())
            {
                try
                {
                    return await channel.Service.FindServices(query, 0, 50);
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public Stream GetContent(string path)
        {
#if DEBUG
            string webClientPath = DebugWebClientPath;
#else
            string webClientPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "www");
#endif
            string file = Path.Combine(webClientPath, path);

            response.ContentType = ContentType.GetType(Path.GetExtension(file));
            return File.Open(file, FileMode.Open);
        }

        public async Task<DefaultConfig> GetDefaultConfig()
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
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
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    return await channel.Service.GetPortalConfig();
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public async Task<QueuePlanMetric> GetQueuePlanMetric(int year, int month, int day, int hour, int minute, int second)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
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
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    return await channel.Service.GetQueuePlanServiceMetric(year, month, day, hours, minute, second, Guid.Parse(serviceId));
                }
                catch (FaultException exception)
                {
                    throw new WebFaultException<string>(exception.Reason.ToString(), HttpStatusCode.BadRequest);
                }
            }
        }

        public virtual Stream Index()
        {
            return GetContent("index.html");
        }
    }
}