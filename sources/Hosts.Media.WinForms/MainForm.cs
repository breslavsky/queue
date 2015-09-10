using Junte.Configuration;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Hosts.Common;
using Queue.Media;
using Queue.Model.Common;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.Hosts.Media.WinForms
{
    public partial class MainForm : Form
    {
        private const string InstallServiceButtonTitle = "Установить службу";
        private const string UnistallServiceButtonTitle = "Удалить службу";
        private const string StartServiceButtonTitle = "Запустить службу";
        private const string StopServiceButtonTitle = "Остановить службу";

        private ServiceManager serviceManager;
        private ConfigurationManager configurationManager;
        private MediaSettings settings;
        private bool started;
        private MediaInstance media;
        private LoginSettings loginSettings;

        public MainForm()
        {
            InitializeComponent();

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostsConsts.MediaServiceExe);
            serviceManager = new ServiceManager(HostsConsts.MediaServiceName, exePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configurationManager = new ConfigurationManager(HostsConsts.MediaApp, Environment.SpecialFolder.CommonApplicationData);
            settings = configurationManager.GetSection<MediaSettings>(HostsConsts.MediaSettingsSectionKey);
            mediaSettingsBindingSource.DataSource = settings;

            RegisterContainer();

            loginSettings = configurationManager.GetSection<LoginSettings>(LoginSettings.SectionKey);
            loginSettingsControl.UserRole = UserRole.Administrator;

            Text += string.Format(" ({0})", typeof(MediaInstance).Assembly.GetName().Version);

            AdjustServiceState();

            serviceStateTimer.Start();
        }

        private void RegisterContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance(configurationManager);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
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
            StartMedia();
        }

        private void serviceStateTimer_Tick(object sender, EventArgs e)
        {
            AdjustServiceState();
        }

        private void selectFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            settings.Folder = folderBrowserDialog.SelectedPath;
            mediaSettingsBindingSource.ResetBindings(false);
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

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                StopMedia();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private async void StartMedia()
        {
            try
            {
                StopMedia();

                startButton.Enabled = false;

                media = new MediaInstance(settings, loginSettings);
                await media.Start();

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

        private void StopMedia()
        {
            if (media == null)
            {
                return;
            }

            media.Stop();
            media = null;

            startButton.Enabled = true;
            stopButton.Enabled = false;

            started = false;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((started) && (!UIHelper.Question("Медиа служба запущена. В случае выхода из программы она будет остановлена. Продолжить?")))
            {
                e.Cancel = true;
                return;
            }

            try
            {
                StopMedia();
            }
            catch { }
        }
    }
}