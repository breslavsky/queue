﻿using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Administrator.Reports;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using QIcons = Queue.UI.Common.Icons;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using Timer = System.Timers.Timer;

namespace Queue.Administrator
{
    public partial class AdministratorForm : RichForm
    {
        private const int PingInterval = 10000;
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IServerService serviceService;
        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly QueueAdministrator currentUser;
        private Channel<IServerTcpService> pingChannel;
        private readonly Timer pingTimer;
        private readonly TaskPool taskPool;

        public AdministratorForm()
            : base()
        {
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();
            currentUser = container.Resolve<User>() as QueueAdministrator;

            channelManager = serviceService.CreateChannelManager(currentUser.SessionId);
            taskPool = new TaskPool();

            pingChannel = channelManager.CreateChannel();

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;

            InitializeComponent();

            Text = currentUserMenuItem.Text = currentUser.ToString();

            CheckPermissions();
        }

        public bool IsLogout { get; private set; }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (pingTimer != null)
                {
                    pingTimer.Dispose();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (pingChannel != null)
                {
                    pingChannel.Dispose();
                }
                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void addClientRequestMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<AddClentRequestForm>(() => new AddClentRequestForm());
        }

        private void CheckPermissions()
        {
            var items = new List<ToolStripItem>(topMenu.Items.Cast<ToolStripItem>());
            items.AddRange(dictionariesMenu.DropDownItems.Cast<ToolStripItem>());
            items.AddRange(clientRequestsMenu.DropDownItems.Cast<ToolStripItem>());

            foreach (ToolStripItem c in items)
            {
                string p = c.Tag as string;
                if (!string.IsNullOrEmpty(p))
                {
                    AdministratorPermissions permission = (AdministratorPermissions)Enum.Parse(typeof(AdministratorPermissions), p);
                    c.Visible = currentUser.Permissions.HasFlag(permission);
                }
            }
        }

        private void clientRequestsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientRequestsForm>(() => new ClientRequestsForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void clientsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientsForm>(() => new ClientsForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void configMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ConfigForm>(() => new ConfigForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void defaultScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<DefaultScheduleForm>(() => new DefaultScheduleForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void exceptionScheduleReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ExceptionScheduleReportForm>(() => new ExceptionScheduleReportForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void logoutMenuItem_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            pingTimer.Stop();
            taskPool.Cancel();
            pingChannel.Close();
        }

        private async void MainForm_Load(object sender, EventArgs eventArgs)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    DefaultConfig config = await taskPool.AddTask(channel.Service.GetDefaultConfig());
                    Text += string.Format(" | {0}", config.QueueName);

                    pingTimer.Start();
                }
                catch (FaultException exception)
                {
                    UIHelper.Warning(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
            }
        }

        private void officesMenuItem_Click(object sender, EventArgs eventArgsventArgs)
        {
            ShowForm<OfficesForm>(() => new OfficesForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void operatorsRatingToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<OperatorRatingReportForm>(() => new OperatorRatingReportForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();
            if (pingTimer.Interval < PingInterval)
            {
                pingTimer.Interval = PingInterval;
            }

            try
            {
                Invoke((MethodInvoker)async delegate
                {
                    try
                    {
                        serverStateLabel.Image = QIcons.connecting16x16;

                        ServerDateTime.Sync(await taskPool.AddTask(pingChannel.Service.GetDateTime()));
                        currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();

                        await taskPool.AddTask(pingChannel.Service.UserHeartbeat());

                        serverStateLabel.Image = QIcons.online16x16;
                    }
                    catch (OperationCanceledException) { }
                    catch (CommunicationObjectAbortedException) { }
                    catch (ObjectDisposedException) { }
                    catch (InvalidOperationException) { }
                    catch (Exception exception)
                    {
                        currentDateTimeLabel.Text = exception.Message;
                        serverStateLabel.Image = QIcons.offline16x16;

                        pingChannel.Dispose();
                        pingChannel = channelManager.CreateChannel();
                    }
                    finally
                    {
                        pingTimer.Start();
                    }
                });
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        private void queueMonitorMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<QueueMonitorForm>(() => new QueueMonitorForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void serviceRatingReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ServiceRatingReportForm>(() => new ServiceRatingReportForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void additionalServiceReportMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<AdditionalServiceRatingReportForm>(() => new AdditionalServiceRatingReportForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void servicesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ServicesForm>(() => new ServicesForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void additionalServicesMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<AdditionalServicesForm>(() => new AdditionalServicesForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void ShowForm<T>(Func<Form> create)
        {
            Form form = MdiChildren.FirstOrDefault(f => f.GetType() == typeof(T));

            if (form != null)
            {
                form.Activate();
                return;
            }

            form = create();
            form.MdiParent = this;

            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void usersMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<UsersForm>(() => new UsersForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void workplacesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<WorkplacesForm>(() => new WorkplacesForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void сurrentScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<CurrentScheduleForm>(() => new CurrentScheduleForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void operatorInterruptionsFormMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<OperatorInterruptionsForm>(() => new OperatorInterruptionsForm(serviceManager.Server.ChannelBuilder, currentUser));
        }

        private void currentUserMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<CurrentUserForm>(() => new CurrentUserForm(serviceManager.Server.ChannelBuilder, currentUser));
        }
    }
}