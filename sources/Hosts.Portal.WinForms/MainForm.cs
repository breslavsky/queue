using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Model.Common;
using Queue.Portal;
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

        private ConfigurationManager configurationManager;
        private PortalSettings settings;
        private TaskPool taskPool;
        private bool started;
        private PortalInstance portal;
        private ServiceManager serviceManager;
        private LoginSettings loginSettings;

        public MainForm()
        {
            InitializeComponent();

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostsConsts.PortalServiceExe);
            serviceManager = new ServiceManager(HostsConsts.PortalServiceName, exePath);

            taskPool = new TaskPool();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configurationManager = new ConfigurationManager(HostsConsts.PortalApp, Environment.SpecialFolder.CommonApplicationData);
            settings = configurationManager.GetSection<PortalSettings>(HostsConsts.PortalSettingsSectionKey);
            portalSettingsBindingSource.DataSource = settings;

            RegisterContainer();

            loginSettings = configurationManager.GetSection<LoginSettings>(LoginSettings.SectionKey);
            loginSettingsControl.Initialize(loginSettings, UserRole.Administrator);

            Text += string.Format(" ({0})", typeof(PortalInstance).Assembly.GetName().Version);

            AdjustServiceState();

            serviceStateTimer.Start();
        }

        private void RegisterContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IConfigurationManager>(configurationManager);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
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
                configurationManager.Save();
                MessageBox.Show("Настройки сохранены");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            StartPortal();
        }

        private async void StartPortal()
        {
            try
            {
                StopPortal();

                startButton.Enabled = false;

                portal = new PortalInstance(settings, loginSettings);
                await portal.Start();

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
            try
            {
                StopPortal();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void StopPortal()
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((started) && (!UIHelper.Question("Портал запущен. В случае выхода из программы он будет остановлен. Продолжить?")))
            {
                e.Cancel = true;
                return;
            }

            try
            {
                StopPortal();
            }
            catch { }
        }
    }
}