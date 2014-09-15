namespace Queue.Services.Common
{
    public class PortalServiceMap
    {
        public const string Index = "/";
        public const string Favicon = "/favicon.ico";
        public const string GetContent = "/content/{*path}";
        public const string ClientAccessPolicy = "clientaccesspolicy.xml";
        public const string GetDefaultConfig = "/config/default";
        public const string GetPortalConfig = "/config/portal";
        public const string FindServices = "/find-services?filter={filter}";
        public const string GetQueuePlanMetric = "/get-queue-plan-metric?year={year}&month={month}&day={day}&hour={hour}&minute={minute}&second={second}";
        public const string GetQueuePlanServiceMetric = "/get-queue-plan-service-metric?year={year}&month={month}&day={day}&hour={hour}&minute={minute}&second={second}&serviceId={serviceId}";
    }
}