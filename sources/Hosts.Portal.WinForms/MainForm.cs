using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Model.Common;
using Queue.Portal;
using System;
using System.Windows.Forms;

namespace Hosts.Portal.WinForms
{
    public partial class MainForm : Form
    {
        private ConfigurationManager configurationManager;
        private PortalSettings settings;
        private TaskPool taskPool;
        private bool started;
        private PortalInstance portal;

        public MainForm()
        {
            InitializeComponent();
            taskPool = new TaskPool();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configurationManager = new ConfigurationManager(AppNames.PortalApp);
            configurationManager.GetSection<PortalSettings>("portal", s => s.Port = 9090);

            RegisterContainer();

            serverConnectionSettingsControl.Initialize(UserRole.Administrator, taskPool);
        }

        private void RegisterContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IConfigurationManager>(configurationManager);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
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

        private void StartPortal()
        {
            try
            {
                startButton.Enabled = false;

                StopPortal();

                portal = new PortalInstance(settings, serverConnectionSettingsControl.ConnectionSettings);
                portal.Start();

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
    }
}