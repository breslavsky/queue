using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;

namespace Queue.Hosts.Common
{
    public class ServiceManager
    {
        private string serviceName;
        private string exePath;

        public ServiceManager(string serviceName, string exePath)
        {
            this.serviceName = serviceName;
            this.exePath = exePath;
        }

        public bool ServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == serviceName);
        }

        public void UnistallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { "/u", exePath });
        }

        public void InstallService()
        {
            ManagedInstallerClass.InstallHelper(new string[] { exePath });
        }

        public bool ServiceRunned()
        {
            if (!ServiceInstalled())
            {
                return false;
            }

            using (ServiceController controller = new ServiceController(serviceName))
            {
                return controller.Status == ServiceControllerStatus.Running;
            }
        }

        public void StartService()
        {
            using (ServiceController controller = new ServiceController(serviceName))
            {
                controller.Start();
            }
        }

        public void StopService()
        {
            using (ServiceController controller = new ServiceController(serviceName))
            {
                controller.Stop();
            }
        }
    }
}