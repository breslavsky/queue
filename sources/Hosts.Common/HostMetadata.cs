namespace Queue.Hosts.Common
{
    public static class HostMetadata
    {
        #region server

        public const string ServerApp = "Queue.Server";
        public const string ServerServiceName = "JunteQueueServer";
        public const string ServerServiceExe = "Queue.Hosts.Server.WinService.exe";

        #endregion server

        #region portal

        public const string PortalApp = "Queue.Portal";
        public const string PortalServiceName = "JunteQueuePortal";
        public const string PortalServiceExe = "Queue.Hosts.Portal.WinService.exe";

        #endregion portal

        #region media

        public const string MediaApp = "Queue.Media";
        public const string MediaServiceName = "JunteQueueMedia";
        public const string MediaServiceExe = "Queue.Hosts.Media.WinService.exe";

        #endregion media

        #region metric

        public const string MetricApp = "Queue.Metric";
        public const string MetricServiceName = "JunteQueueMetric";
        public const string MetricServiceExe = "Queue.Hosts.Metric.WinService.exe";

        #endregion metric

        #region hub

        public const string HubApp = "Queue.Hub";
        public const string HubServiceName = "JunteQueueHub";
        public const string HubServiceExe = "Queue.Hosts.Hub.WinService.exe";

        #endregion hub
    }
}