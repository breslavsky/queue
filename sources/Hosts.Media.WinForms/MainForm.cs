using Junte.Configuration;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common.Settings;
using Queue.Hosts.Common;
using Queue.Media;
using Queue.Model.Common;
using Queue.Services.Media.Settings;
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

        private ConfigurationManager configuration;
        private bool started;
        private MediaInstance media;
        private ServiceManager serviceManager;
        private MediaSettings settings;
        private MediaServiceSettings mediaServiceSettings;

        public MainForm()
        {
            InitializeComponent();

            var container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostsConsts.MediaServiceExe);
            serviceManager = new ServiceManager(HostsConsts.MediaServiceName, exePath);

            configuration = new ConfigurationManager(HostsConsts.MediaApp, Environment.SpecialFolder.CommonApplicationData);
            container.RegisterInstance(configuration);

            settings = configuration.GetSection<MediaSettings>(MediaSettings.SectionKey);
            container.RegisterInstance(settings);

            mediaSettingsBindingSource.DataSource = settings;

            mediaServiceSettings = configuration.GetSection<MediaServiceSettings>(MediaServiceSettings.SectionKey);
            container.RegisterInstance(mediaServiceSettings);

            mediaServiceSettingsBindingSource.DataSource = mediaServiceSettings;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", typeof(MediaInstance).Assembly.GetName().Version);

            AdjustServiceState();

            serviceStateTimer.Start();
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
            StartMedia();
        }

        private void serviceStateTimer_Tick(object sender, EventArgs e)
        {
            AdjustServiceState();
        }

        private void selectMediaFolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                mediaServiceSettings.MediaFolder = folderBrowserDialog.SelectedPath;
                mediaServiceSettingsBindingSource.ResetBindings(false);
            }
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

        private void StartMedia()
        {
            try
            {
                StopMedia();

                startButton.Enabled = false;

                media = new MediaInstance(settings);
                media.Start();

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