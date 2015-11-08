using Junte.Configuration;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Server;
using Queue.Server.Settings;
using Queue.Services.Server.Settings;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.Hosts.Server.WinForms
{
    public partial class MainForm : RichForm
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const string InstallServiceButtonTitle = "Установить службу";
        private const string UnistallServiceButtonTitle = "Удалить службу";
        private const string StartServiceButtonTitle = "Запустить службу";
        private const string StopServiceButtonTitle = "Остановить службу";

        private readonly ConfigurationManager configuration;
        private readonly ServerSettings settings;
        private readonly TemplateServiceSettings templateServiceSettings;
        private readonly ServiceManager serviceManager;
        private ServerInstance server;
        private bool started;

        public MainForm()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostMetadata.ServerServiceExe);
            serviceManager = new ServiceManager(HostMetadata.ServerServiceName, exePath);

            configuration = new ConfigurationManager(HostMetadata.ServerApp, Environment.SpecialFolder.CommonApplicationData);
            container.RegisterInstance(configuration);

            var commonApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            logger.Info(commonApplicationData);

            templateServiceSettings = configuration.GetSection<TemplateServiceSettings>(TemplateServiceSettings.SectionKey);
            container.RegisterInstance(templateServiceSettings);

            settings = configuration.GetSection<ServerSettings>(ServerSettings.SectionKey);
            container.RegisterInstance(settings);

            editDatabaseSettingsControl.Settings = settings.Database;
            languageControl.Initialize<Language>();
            languageControl.Select<Language>(settings.Language);
            licenseTypeControl.Initialize<ProductLicenceType>();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", typeof(ServerInstance).Assembly.GetName().Version);

            AdjustServiceSettings();
            AdjustServiceState();
            AdjustLicenseSettings();

            serviceStateTimer.Start();
        }

        private void serviceStateTimer_Tick(object sender, EventArgs e)
        {
            AdjustServiceState();
        }

        private void AdjustServiceSettings()
        {
            TcpServiceConfig tcp = settings.Services.TcpService;

            tcpCheckBox.Checked = tcp.Enabled;
            tcpHostTextBox.Text = tcp.Host;
            tcpPortUpDown.Value = tcp.Port;

            HttpServiceConfig http = settings.Services.HttpService;

            httpCheckBox.Checked = http.Enabled;
            httpHostTextBox.Text = http.Host;
            httpPortUpDown.Value = http.Port;
        }

        private void AdjustLicenseSettings()
        {
            ProductLicenceConfig licence = settings.Licence;

            licenseTypeControl.Select<ProductLicenceType>(licence.LicenseType);
            serialKeyTextBox.Text = licence.SerialKey;
            registerKeyTextBox.Text = licence.RegisterKey;
        }

        private void AdjustServiceState()
        {
            bool serviceInstalled = serviceManager.ServiceInstalled();

            installServiseButton.Text = serviceInstalled ? UnistallServiceButtonTitle
                 : InstallServiceButtonTitle;

            startServiceButton.Enabled = serviceInstalled;

            bool runned = serviceManager.ServiceRunned();
            startServiceButton.Text = runned ? StopServiceButtonTitle
                : StartServiceButtonTitle;

            serviceStatePicture.Image = runned ? Icons.online
                : Icons.offline;

            startButton.Enabled = !started && !runned;
            stopButton.Enabled = started && !runned;
        }

        private void startButton_Click(object sender, EventArgs eventArgs)
        {
            StartServer();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                StopServer();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                editDatabaseSettingsControl.Save();
                configuration.Save();
                MessageBox.Show("Настройки сохранены");
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                StopServer();
            }
            catch { }
        }

        private void StartServer()
        {
            try
            {
                startButton.Enabled = false;

                StopServer();

                editDatabaseSettingsControl.Save();
                server = new ServerInstance(settings);
                server.Start();

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

        private void StopServer()
        {
            if (server == null)
            {
                return;
            }

            server.Stop();
            server = null;

            startButton.Enabled = true;
            stopButton.Enabled = false;

            started = false;
        }

        private void languageControl_SelectedChanged(object sender, EventArgs e)
        {
            Language language = languageControl.Selected<Language>();
            language.SetCurrent();

            Translate();

            settings.Language = language;
        }

        #region bindings

        private void tcpHostTextBox_Leave(object sender, EventArgs e)
        {
            settings.Services.TcpService.Host = tcpHostTextBox.Text;
        }

        private void tcpPortUpDown_Leave(object sender, EventArgs e)
        {
            settings.Services.TcpService.Port = (int)tcpPortUpDown.Value;
        }

        private void tcpCheckBox_Leave(object sender, EventArgs e)
        {
            settings.Services.TcpService.Enabled = tcpCheckBox.Checked;
        }

        private void tcpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            tcpGroupBox.Enabled = tcpCheckBox.Checked;
        }

        private void httpHostTextBox_Leave(object sender, EventArgs e)
        {
            settings.Services.HttpService.Host = httpHostTextBox.Text;
        }

        private void httpPortUpDown_Leave(object sender, EventArgs e)
        {
            settings.Services.HttpService.Port = (int)httpPortUpDown.Value;
        }

        private void httpCheckBox_Leave(object sender, EventArgs e)
        {
            settings.Services.HttpService.Enabled = httpCheckBox.Checked;
        }

        private void httpCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            httpGroupBox.Enabled = httpCheckBox.Checked;
        }

        private void licenseTypeControl_Leave(object sender, EventArgs e)
        {
            settings.Licence.LicenseType = licenseTypeControl.Selected<ProductLicenceType>();
        }

        private void serialKeyTextBox_Leave(object sender, EventArgs e)
        {
            settings.Licence.SerialKey = serialKeyTextBox.Text;
        }

        private void registerKeyTextBox_Leave(object sender, EventArgs e)
        {
            settings.Licence.RegisterKey = registerKeyTextBox.Text;
        }

        #endregion bindings

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