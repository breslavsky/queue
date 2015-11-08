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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Formatting = Newtonsoft.Json.Formatting;

namespace Queue.Hosts.Hub.WinForms
{
    public partial class MainForm : RichForm
    {
        private const string InstallServiceButtonTitle = "Установить службу";
        private const string UnistallServiceButtonTitle = "Удалить службу";
        private const string StartServiceButtonTitle = "Запустить службу";
        private const string StopServiceButtonTitle = "Остановить службу";

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ConfigurationManager configuration;
        private readonly ServiceManager serviceManager;
        private HubSettings settings;
        private HubInstance hub;
        private bool started;

        public MainForm()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(HostMetadata.HubApp, Environment.SpecialFolder.CommonApplicationData);
            container.RegisterInstance(configuration);

            settings = configuration.GetSection<HubSettings>(HubSettings.SectionKey);
            container.RegisterInstance(settings);

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostMetadata.HubServiceExe);
            serviceManager = new ServiceManager(HostMetadata.HubServiceName, exePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", typeof(HubInstance).Assembly.GetName().Version);

            AdjustServiceState();

            serviceStateTimer.Start();

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

        private void startButton_Click(object sender, EventArgs e)
        {
            StartHub();
        }

        private void serviceStateTimer_Tick(object sender, EventArgs e)
        {
            AdjustServiceState();
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

        private void installServiceButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (serviceManager.ServiceInstalled())
                {
                    serviceManager.UnistallService();
                    MessageBox.Show("Служба удалена");
                }
                else
                {
                    serviceManager.InstallService();
                    MessageBox.Show("Служба установлена");
                }
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }

            AdjustServiceState();
        }

        private void AdjustServiceState()
        {
            bool serviceInstalled = serviceManager.ServiceInstalled();

            installServiceButton.Text = serviceInstalled ?
                 UnistallServiceButtonTitle :
                 InstallServiceButtonTitle;

            startServiceButton.Enabled = serviceInstalled;

            bool runned = serviceManager.ServiceRunned();
            startServiceButton.Text = runned ?
                                        StopServiceButtonTitle :
                                        StartServiceButtonTitle;

            serviceStatePicture.Image = runned ?
                                            Icons.online :
                                            Icons.offline;

            startButton.Enabled = !started && !runned;
            stopButton.Enabled = started && !runned;
        }

        private void startServiceButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (serviceManager.ServiceRunned())
                {
                    serviceManager.StopService();
                    MessageBox.Show("Служба остановлена");
                }
                else
                {
                    serviceManager.StartService();
                    MessageBox.Show("Служба запущена");
                }
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }

            AdjustServiceState();
        }
    }
}