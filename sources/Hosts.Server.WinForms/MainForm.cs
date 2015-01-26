using Junte.Data.NHibernate;
using Junte.UI.WinForms;
using log4net;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Hosts.Server.WinForms.Properties;
using Queue.Server;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.Hosts.Server.WinForms
{
    public partial class MainForm : Form
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        private const string ServiceName = "JunteQueueServer";
        private const string ServiceExe = "Queue.Hosts.Server.WinService.exe";

        private const string InstallServiceButtonTitle = "Установить сервис";
        private const string UnistallServiceButtonTitle = "Удалить сервис";
        private const string StartServiceButtonTitle = "Запустить сервис";
        private const string StopServiceButtonTitle = "Остановить сервис";

        private ConfigurationManager configuration;
        private ServerSettings settings;
        private ServerInstance server;
        private ServiceManager serviceManager;
        private bool started;

        public MainForm()
        {
            InitializeComponent();

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), ServiceExe);
            serviceManager = new ServiceManager(ServiceName, exePath);
            LoadConfiguration();

            editDatabaseSettingsControl.Settings = settings.Database;
            debugCheckBox.Checked = settings.Debug;
        }

        private void LoadConfiguration()
        {
            configuration = new ConfigurationManager(AppNames.ServerApp);
            settings = configuration.GetSection<ServerSettings>("server", (s) =>
                                                                            {
                                                                                s.Database = GetDefaultDatabaseSettings();
                                                                                s.Services = GetDefaultServicesConfig();
                                                                                s.Debug = true;
                                                                            });
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
                                Enabled = false,
                                Host = "localhost",
                                Port = 4506
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
            Text += string.Format(" ({0})", typeof(ServerInstance).Assembly.GetName().Version);

            AdjustServiceSettings();
            AdjustServiceState();

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

        private void AdjustServiceState()
        {
            bool serviceInstalled = serviceManager.ServiceInstalled();

            installServiseButton.Text = serviceInstalled ?
                 UnistallServiceButtonTitle :
                 InstallServiceButtonTitle;

            runServiceButton.Enabled = serviceInstalled;

            bool runned = serviceManager.ServiceRunned();
            runServiceButton.Text = runned ?
                                        StopServiceButtonTitle :
                                        StartServiceButtonTitle;

            serviceStatePicture.Image = runned ?
                                            Resources.online.ToBitmap() :
                                            Resources.offline.ToBitmap();

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
            if ((started) && (!UIHelper.Question("Сервер запущен. В случае выхода из программы он будет остановлен. Продолжить?")))
            {
                e.Cancel = true;
                return;
            }

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

        private void runServiceButton_Click(object sender, EventArgs e)
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}