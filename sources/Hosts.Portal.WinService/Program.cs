using System.ServiceProcess;

namespace Hosts.Portal.WinService
{
    internal static class Program
    {
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new PortalService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}