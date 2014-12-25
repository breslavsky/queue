using Junte.Data.NHibernate;
using Junte.UI.WinForms;
using Queue.Server;
using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;

namespace Queue.Hosts.Server.WinForms
{
    public partial class MainForm : Form
    {
        private const string InstallServiceButtonTitle = "Установить сервис";
        private const string UnistallServiceButtonTitle = "Удалить сервис";
        private const string StartServiceButtonTitle = "Запустить сервис";
        private const string StopServiceButtonTitle = "Остановить сервис";

        private Configuration configuration;
        private ServerSettings settings;
        private ServerInstance server;
        private ServerServiceManager serviceManager;

        public MainForm()
        {
            InitializeComponent();

            serviceManager = new ServerServiceManager();

            configuration = GetConfiguration();
            settings = configuration.GetSection("server") as ServerSettings;
            if (settings == null)
            {
                settings = new ServerSettings()
                    {
                        Database = GetDefaultDatabaseSettings(),
                        Services = GetDefaultServicesConfig(),
                        Debug = true
                    };
                configuration.Sections.Add("server", settings);
            }

            editDatabaseSettingsControl.Settings = settings.Database;

            debugCheckBox.Checked = settings.Debug;
        }

        private Configuration GetConfiguration()
        {
            ExeConfigurationFileMap configMap = new ExeConfigurationFileMap();
            configMap.ExeConfigFilename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                                                    "Junte",
                                                                    "Queue.Server",
                                                                    "app.config");

            return ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
        }

        private DatabaseSettings GetDefaultDatabaseSettings()
        {
            return new DatabaseSettings()
                        {
                            Server = "localhost",
                            Name = "queue",
                            Type = DatabaseType.MsSql,
                            Integrated = true
                        };
        }

        private ServicesConfig GetDefaultServicesConfig()
        {
            return new ServicesConfig()
                        {
                            HttpService = new HttpServiceConfig()
                            {
                                Enabled = false
                            },
                            TcpService = new TcpServiceConfig()
                            {
                                Enabled = true,
                                Host = "localhost",
                                Port = 4505
                            }
                        };
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            AdjustServiceSettings();
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

        private void AdjustServiceState()
        {
            bool serviceInstalled = serviceManager.ServiceInstalled();

            installServiseButton.Text = serviceInstalled ?
                 UnistallServiceButtonTitle :
                 InstallServiceButtonTitle;

            runServiceButton.Enabled = serviceInstalled;
            runServiceButton.Text = serviceManager.ServiceRunned() ?
                                        StopServiceButtonTitle :
                                        StartServiceButtonTitle;
        }

        private void startButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                server = new ServerInstance(settings);
                server.Start();

                configuration.Save();

                panel.Enabled = false;
            }
            catch (Exception e)
            {
                UIHelper.Error(e);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
            {
                server.Stop();
            }
        }

        #region bindings

        private void debugCheckBox_Leave(object sender, EventArgs e)
        {
            settings.Debug = debugCheckBox.Checked;
        }

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

        #endregion bindings

        private void installServiseButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (serviceManager.ServiceInstalled())
                {
                    serviceManager.UnistallService();
                    MessageBox.Show("Сервис удален");
                }
                else
                {
                    serviceManager.InstallService();
                    MessageBox.Show("Сервис установлен");
                }
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }

            AdjustServiceState();
        }

        private void runServiceButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (serviceManager.ServiceRunned())
                {
                    serviceManager.StopService();
                    MessageBox.Show("Сервис остановлен");
                }
                else
                {
                    serviceManager.StartService();
                    MessageBox.Show("Сервис запущен");
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