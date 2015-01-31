using System.ServiceProcess;

namespace Queue.Hosts.Media.WinService
{
    internal static class Program
    {
        private static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MediaService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}