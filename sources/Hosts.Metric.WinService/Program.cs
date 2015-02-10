using System.ServiceProcess;

namespace Queue.Hosts.Metric.WinService
{
    internal static class Program
    {
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MetricService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}