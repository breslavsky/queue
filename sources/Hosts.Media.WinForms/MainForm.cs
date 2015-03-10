﻿using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Hosts.Common;
using Queue.Hosts.Media.WinForms.Properties;
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
        private TaskPool taskPool;
        private ConfigurationManager configurationManager;
        private MediaSettings settings;
        private bool started;
        private MediaInstance media;

        public MainForm()
        {
            InitializeComponent();

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostsConsts.MediaServiceExe);
            serviceManager = new ServiceManager(HostsConsts.MediaServiceName, exePath);

            taskPool = new TaskPool();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            configurationManager = new ConfigurationManager(HostsConsts.MediaApp);
            settings = configurationManager.GetSection<MediaSettings>(HostsConsts.MediaSettingsSectionKey);
            mediaSettingsBindingSource.DataSource = settings;

            RegisterContainer();

            loginSettingsControl.Initialize(UserRole.Administrator, taskPool);

            Text += string.Format(" ({0})", typeof(MediaInstance).Assembly.GetName().Version);

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

            installServiceButton.Text = serviceInstalled ?
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

        private void saveButton_Click(object sender, System.EventArgs e)
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

        private void serviceStateTimer_Tick(object sender, System.EventArgs e)
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

                media = new MediaInstance(settings, loginSettingsControl.LoginSettings);
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

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
            }
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }
    }
}