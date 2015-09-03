using Junte.Configuration;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using NLog;
using Queue.Hosts.Common;
using Queue.Hub;
using Queue.Hub.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace Hosts.Hub.WinForms
{
    public partial class MainForm : RichForm
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private ConfigurationManager configurationManager;
        private HubInstance hub;
        private HubSettings settings;
        private bool started;

        public MainForm()
        {
            InitializeComponent();

            configurationManager = new ConfigurationManager(HostsConsts.HubApp, Environment.SpecialFolder.CommonApplicationData);
            settings = configurationManager.GetSection<HubSettings>(HubSettings.SectionKey);

            var tcpService = settings.Services.TcpService;
            var httpService = settings.Services.HttpService;

            var export = new
            {
                tcp = new
                {
                    enabled = tcpService.Enabled,
                    host = tcpService.Host,
                    port = tcpService.Port
                },
                http = new
                {
                    enabled = httpService.Enabled,
                    host = httpService.Host,
                    port = httpService.Port
                },
                drivers = new
                {
                    display = new List<string>(),
                    quality = new List<string>()
                }
            };

            var display = settings.Drivers.Display;
            foreach (DriverElementConfig d in display)
            {
                export.drivers.display.Add(d.Config.Type);
            }

            var quality = settings.Drivers.Quality;
            foreach (DriverElementConfig d in quality)
            {
                export.drivers.quality.Add(d.Config.Type);
            }

            settingsTextBox.Text = JsonConvert.SerializeObject(export, Formatting.Indented);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RegisterContainer();

            Text += string.Format(" ({0})", typeof(HubInstance).Assembly.GetName().Version);
        }

        private void RegisterContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterInstance<IConfigurationManager>(configurationManager);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartHub();
        }

        private void StartHub()
        {
            try
            {
                StopHub();

                startButton.Enabled = false;

                hub = new HubInstance(settings);
                hub.Start();

                startButton.Enabled = false;
                stopButton.Enabled = true;
                started = true;
            }
            catch (Exception e)
            {
                startButton.Enabled = true;
                UIHelper.Error(e);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopHub();
        }

        private void StopHub()
        {
            if (hub == null)
            {
                return;
            }

            hub.Stop();
            hub = null;

            startButton.Enabled = true;
            stopButton.Enabled = false;

            started = false;
        }
    }
}