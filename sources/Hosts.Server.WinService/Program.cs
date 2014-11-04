using log4net.Config;
using System.ServiceProcess;

namespace Queue.Hosts.Server.WinService
{
    internal static class Program
    {
        private static void Main()
        {
            XmlConfigurator.Configure();

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ServerService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}