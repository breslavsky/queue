using System.Configuration.Install;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace Queue.Hosts.Server.WinForms
{
    public class ServerServiceManager
    {
        private const string ServiceName = "JunteQueueServer";
        private const string ServiceDescription = "Junte Queue Server";
        private const string ServiceDisplayName = "Junte Queue Server";

        private const string ServiceExe = "Queue.Hosts.Server.WinService.exe";

        public bool ServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == ServiceName);
        }

        internal void UnistallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", GetServiceExeLocation() });
        }

        internal void InstallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { GetServiceExeLocation() });
        }

        internal bool ServiceRunned()
        {
            if (!ServiceInstalled())
            {
                return false;
            }

            using (ServiceController controller = new ServiceController(ServiceName))
            {
                return controller.Status == ServiceControllerStatus.Running;
            }
        }

        internal void StartService()
        {
            using (ServiceController controller = new ServiceController(ServiceName))
            {
                controller.Start();
            }
        }

        internal void StopService()
        {
            using (ServiceController controller = new ServiceController(ServiceName))
            {
                controller.Stop();
            }
        }

        private string GetServiceExeLocation()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ServiceExe);
        }
    }
}