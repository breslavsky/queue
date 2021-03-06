﻿using Junte.Configuration;
using Junte.UI.WinForms;
using NLog;
using Queue.Hosts.Common;
using Queue.Metric;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Queue.Hosts.Metric.WinForms
{
    public partial class MainForm : RichForm
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private const string InstallServiceButtonTitle = "Установить службу";
        private const string UnistallServiceButtonTitle = "Удалить службу";
        private const string StartServiceButtonTitle = "Запустить службу";
        private const string StopServiceButtonTitle = "Остановить службу";

        private bool started;

        private ConfigurationManager configuration;
        private ServiceManager serviceManager;
        private MetricSettings settings;
        private MetricInstance metric;

        public MainForm()
        {
            InitializeComponent();

            string exePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), HostMetadata.MetricServiceExe);
            serviceManager = new ServiceManager(HostMetadata.MetricServiceName, exePath);
            LoadConfiguration();

            editDatabaseSettingsControl.Settings = settings.Database;
        }

        private void LoadConfiguration()
        {
            configuration = new ConfigurationManager(HostMetadata.MetricApp, Environment.SpecialFolder.CommonApplicationData);
            settings = configuration.GetSection<MetricSettings>(MetricSettings.SectionKey);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", typeof(MetricInstance).Assembly.GetName().Version);

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
                editDatabaseSettingsControl.Save();
                configuration.Save();
                MessageBox.Show("Настройки сохранены");
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
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

        private void serviceStateTimer_Tick(object sender, EventArgs e)
        {
            AdjustServiceState();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            try
            {
                startButton.Enabled = false;

                Stop();

                editDatabaseSettingsControl.Save();
                metric = new MetricInstance(settings);
                metric.Start();

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

        private void Stop()
        {
            if (metric == null)
            {
                return;
            }

            metric.Stop();
            metric = null;

            startButton.Enabled = true;
            stopButton.Enabled = false;

            started = false;
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

        private void stopButton_Click(object sender, EventArgs e)
        {
            try
            {
                Stop();
            }
            catch (Exception ex)
            {
                UIHelper.Error(ex);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((started) && (!UIHelper.Question("Сбор метрик запущен. В случае выхода из программы он будет остановлен. Продолжить?")))
            {
                e.Cancel = true;
                return;
            }

            try
            {
                Stop();
            }
            catch { }
        }
    }
}