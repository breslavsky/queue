﻿using Queue.Hosts.Common;
using System.ComponentModel;
using System.Configuration.Install;

namespace Queue.Hosts.Metric.WinService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();

            serviceInstaller.ServiceName = HostMetadata.MetricServiceName;
        }
    }
}