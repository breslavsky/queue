using Queue.Hosts.Common;
using System.ComponentModel;
using System.Configuration.Install;

namespace Queue.Hosts.Portal.WinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            serviceInstaller.ServiceName = HostsConsts.PortalServiceName;
        }
    }
}