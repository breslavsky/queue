using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Administrator.Reports;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
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
    public partial class AdministratorForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public ChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IUserTcpService> ServerUserChannelManager { get; set; }

        #endregion dependency

        #region fields

        private const int ServerDateTimeTimerInterval = 60000;
        private const int CurrentDateTimerInterval = 1000;
        private const int HeartbeatTimerInterval = 5000;

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Timer currentDateTimeTimer = new Timer();
        private readonly Timer serverDateTimeTimer = new Timer();
        private readonly Timer heartbeatTimer = new Timer();
        private readonly TaskPool taskPool = new TaskPool();

        #endregion fields

        #region properties

        public bool IsLogout { get; private set; }

        #endregion properties

        public AdministratorForm()
            : base()
        {
            InitializeComponent();

            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            currentDateTimeTimer.Elapsed += currentDateTimeTimer_Elapsed;
            serverDateTimeTimer.Elapsed += serverDateTimeTimer_Elapsed;
            heartbeatTimer.Elapsed += heartbeatTimer_Elapsed;

            CheckPermissions();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                if (currentDateTimeTimer != null)
                {
                    currentDateTimeTimer.Dispose();
                }

                if (serverDateTimeTimer != null)
                {
                    serverDateTimeTimer.Dispose();
                }
                if (heartbeatTimer != null)
                {
                    heartbeatTimer.Dispose();
                }

                if (taskPool != null)
                {
                    taskPool.Dispose();
                }

                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }
                if (ServerUserChannelManager != null)
                {
                    ServerUserChannelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region form events

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            serverDateTimeTimer.Stop();
            currentDateTimeTimer.Stop();
            heartbeatTimer.Stop();
            taskPool.Cancel();
        }

        private async void MainForm_Load(object sender, EventArgs eventArgs)
        {
            Text = currentUserMenuItem.Text = CurrentUser.ToString();
            Controls.OfType<MdiClient>().First().BackgroundImage = Properties.Resources.background;

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    var config = await taskPool.AddTask(channel.Service.GetDefaultConfig());
                    Text += string.Format(" | {0}", config.QueueName);
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

            currentDateTimeTimer.Start();
            serverDateTimeTimer.Start();
            heartbeatTimer.Start();
        }

        #endregion form events

        #region taskpool

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        #endregion taskpool

        #region timers

        private void currentDateTimeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            currentDateTimeTimer.Stop();
            if (currentDateTimeTimer.Interval < CurrentDateTimerInterval)
            {
                currentDateTimeTimer.Interval = CurrentDateTimerInterval;
            }

            try
            {
                Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();
                    }
                    catch (Exception exception)
                    {
                        logger.Error(exception);
                    }
                    finally
                    {
                        currentDateTimeTimer.Start();
                    }
                });
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        private async void serverDateTimeTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            serverDateTimeTimer.Stop();
            if (serverDateTimeTimer.Interval < ServerDateTimeTimerInterval)
            {
                serverDateTimeTimer.Interval = ServerDateTimeTimerInterval;
            }

            try
            {
                using (var channel = ChannelManager.CreateChannel())
                {
                    ServerDateTime.Sync(await taskPool.AddTask(channel.Service.GetDateTime()));
                }
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
            finally
            {
                serverDateTimeTimer.Start();
            }
        }

        private void heartbeatTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            heartbeatTimer.Stop();
            if (heartbeatTimer.Interval < HeartbeatTimerInterval)
            {
                heartbeatTimer.Interval = HeartbeatTimerInterval;
            }

            try
            {
                Invoke((MethodInvoker)async delegate
                {
                    try
                    {
                        serverStateLabel.Image = QIcons.connecting16x16;

                        using (var channel = ServerUserChannelManager.CreateChannel())
                        {
                            await taskPool.AddTask(channel.Service.UserHeartbeat());
                        }

                        serverStateLabel.Image = QIcons.online16x16;
                    }
                    catch
                    {
                        serverStateLabel.Image = QIcons.offline16x16;
                    }
                    finally
                    {
                        heartbeatTimer.Start();
                    }
                });
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        #endregion timers

        private void CheckPermissions()
        {
            var items = new List<ToolStripItem>(topMenu.Items.Cast<ToolStripItem>());
            items.AddRange(dictionariesMenu.DropDownItems.Cast<ToolStripItem>());
            items.AddRange(clientRequestsMenu.DropDownItems.Cast<ToolStripItem>());
            items.AddRange(queuePlanMenu.DropDownItems.Cast<ToolStripItem>());

            foreach (ToolStripItem c in items)
            {
                string p = c.Tag as string;
                if (!string.IsNullOrEmpty(p))
                {
                    AdministratorPermissions permission = (AdministratorPermissions)Enum.Parse(typeof(AdministratorPermissions), p);
                    c.Visible = CurrentUser.Permissions.HasFlag(permission);
                }
            }
        }

        private void ShowForm<T>(Func<Form> create)
        {
            var form = MdiChildren.FirstOrDefault(f => f.GetType() == typeof(T));
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

        #region top menu

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void addClientRequestMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<AddClentRequestForm>(() => new AddClentRequestForm());
        }

        private void additionalServiceReportMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<AdditionalServiceRatingReportForm>(() => new AdditionalServiceRatingReportForm());
        }

        private void additionalServicesMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<AdditionalServicesForm>(() => new AdditionalServicesForm());
        }

        private void clientRequestsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientRequestsForm>(() => new ClientRequestsForm());
        }

        private void clientsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientsForm>(() => new ClientsForm());
        }

        private void configMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ConfigForm>(() => new ConfigForm());
        }

        private void currentUserMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<CurrentUserForm>(() => new CurrentUserForm());
        }

        private void defaultScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<DefaultScheduleForm>(() => new DefaultScheduleForm());
        }

        private void exceptionScheduleReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ExceptionScheduleReportForm>(() => new ExceptionScheduleReportForm());
        }

        private void officesMenuItem_Click(object sender, EventArgs eventArgsventArgs)
        {
            ShowForm<OfficesForm>(() => new OfficesForm());
        }

        private void operatorInterruptionsFormMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<OperatorInterruptionsForm>(() => new OperatorInterruptionsForm());
        }

        private void operatorsRatingToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<OperatorRatingReportForm>(() => new OperatorRatingReportForm());
        }

        private void queueMonitorMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<QueueMonitorForm>(() => new QueueMonitorForm());
        }

        private void serviceRatingReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ServiceRatingReportForm>(() => new ServiceRatingReportForm());
        }

        private void servicesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ServicesForm>(() => new ServicesForm());
        }

        private void lifeSituationsMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm<LifeSituationsForm>(() => new LifeSituationsForm());
        }

        private void usersMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<UsersForm>(() => new UsersForm());
        }

        private void workplacesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<WorkplacesForm>(() => new WorkplacesForm());
        }

        private void сurrentScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<CurrentScheduleForm>(() => new CurrentScheduleForm());
        }

        private void logoutMenuItem_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }

        #endregion top menu
    }
}