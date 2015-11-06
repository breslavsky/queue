using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Portal;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Services.Portal.Settings;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Services.Portal
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public class PortalService : DependencyService, IPortalService
    {
        #region dependency

        [Dependency]
        public PortalServiceSettings Settings { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public DuplexChannelManager<IQueuePlanTcpService> QueuePlanChannelManager { get; set; }

        #endregion dependency

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        protected readonly IncomingWebRequestContext request;
        protected readonly OutgoingWebResponseContext response;

        public PortalService()
        {
            request = WebOperationContext.Current.IncomingRequest;
            response = WebOperationContext.Current.OutgoingResponse;
        }

        public Stream Favicon()
        {
            return GetContent("favicon.ico");
        }

        public async Task<Service[]> FindServices(string query)
        {
            using (var channel = ChannelManager.CreateChannel())
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
            string file = Path.Combine(Settings.ContentFolder, path);

            response.ContentType = ContentType.GetType(Path.GetExtension(file));
            return File.Open(file, FileMode.Open);
        }

        public async Task<DefaultConfig> GetDefaultConfig()
        {
            using (var channel = ChannelManager.CreateChannel())
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
            using (var channel = ChannelManager.CreateChannel())
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
            using (var channel = ChannelManager.CreateChannel())
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
            using (var channel = ChannelManager.CreateChannel())
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