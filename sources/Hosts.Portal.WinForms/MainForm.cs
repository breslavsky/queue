using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Hosts.Common;
using Queue.Model.Common;
using Queue.Portal;
using Queue.Services.Portal.Settings;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Hosts.Portal.WinForms
{
    public partial class MainForm : Form
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const string InstallServiceButtonTitle = "Установить службу";
        private const string UnistallServiceButtonTitle = "Удалить службу";
        private const string StartServiceButtonTitle = "Запустить службу";
        private const string StopServiceButtonTitle = "Остановить службу";

        private ConfigurationManager configuration;
        private bool started;
        private PortalInstance portal;
        private ServiceManager serviceManager;
        private PortalSettings settings;
        private LoginSettings loginSettings;
        private PortalServiceSettings portalServiceSettings;

        public MainForm()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostsConsts.PortalServiceExe);
            serviceManager = new ServiceManager(HostsConsts.PortalServiceName, exePath);

            configuration = new ConfigurationManager(HostsConsts.PortalApp, Environment.SpecialFolder.CommonApplicationData);
            container.RegisterInstance(configuration);

            settings = configuration.GetSection<PortalSettings>(PortalSettings.SectionKey);
            container.RegisterInstance(settings);

            portalSettingsBindingSource.DataSource = settings;

            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            container.RegisterInstance(loginSettings);

            loginSettingsControl.Settings = loginSettings;

            portalServiceSettings = configuration.GetSection<PortalServiceSettings>(PortalServiceSettings.SectionKey);
            container.RegisterInstance(portalServiceSettings);

            portalServiceSettingsBindingSource.DataSource = portalServiceSettings;

            loginSettingsControl.UserRole = UserRole.Administrator;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", typeof(PortalInstance).Assembly.GetName().Version);

            AdjustServiceState();

            serviceStateTimer.Start();
        }

        private void AdjustServiceState()
        {
            bool serviceInstalled = serviceManager.ServiceInstalled();

            installServiseButton.Text = serviceInstalled ?
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                configuration.Save();
                MessageBox.Show("Настройки сохранены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                startButton.Enabled = false;

                portal = new PortalInstance(settings, loginSettings);
                portal.Start();

                startButton.Enabled = false;
                stopButton.Enabled = true;
                started = true;
            }
            catch (Exception ex)
            {
                startButton.Enabled = true;
                UIHelper.Error(ex);
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (portal == null)
            {
                return;
            }

            portal.Stop();
            portal = null;

            startButton.Enabled = true;
            stopButton.Enabled = false;

            started = false;
        }

        private void installServiseButton_Click(object sender, EventArgs e)
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

        private void serviceStateTimer_Tick(object sender, EventArgs e)
        {
            AdjustServiceState();
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

        private void selectContentFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                portalServiceSettings.ContentFolder = folderBrowserDialog.SelectedPath;
                portalServiceSettingsBindingSource.ResetBindings(false);
            }
        }
    }
}