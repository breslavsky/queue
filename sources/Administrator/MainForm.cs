using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using log4net;
using Queue.Common;
using Queue.Manager;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using QIcons = Queue.UI.Common.Icons;
using Timer = System.Timers.Timer;

namespace Queue.Administrator
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Channel<IServerTcpService> pingChannel;

        private Timer pingTimer;
        private int PING_INTERVAL = 10000;

        public MainForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            Text = currentUser.ToString();

            pingChannel = channelManager.CreateChannel();

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;
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

        private void pingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();
            if (pingTimer.Interval < PING_INTERVAL)
            {
                pingTimer.Interval = PING_INTERVAL;
            }

            try
            {
                Invoke((MethodInvoker)async delegate
                {
                    try
                    {
                        serverStateLabel.Image = QIcons.connecting16x16;

                        if (!pingChannel.IsConnected)
                        {
                            await taskPool.AddTask(pingChannel.Service.OpenUserSession(currentUser.SessionId));
                        }
                        else
                        {
                            await taskPool.AddTask(pingChannel.Service.UserHeartbeat());
                        }

                        ServerDateTime.Sync(await taskPool.AddTask(pingChannel.Service.GetDateTime()));
                        currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();

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
            catch (Exception)
            {
                // disposed
            }
        }

        private async void MainForm_Load(object sender, EventArgs eventArgs)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
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

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void configMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ConfigForm>(() => new ConfigForm(channelBuilder, currentUser));
        }

        private void defaultScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<DefaultScheduleForm>(() => new DefaultScheduleForm(channelBuilder, currentUser));
        }

        private void workplacesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<WorkplacesForm>(() => new WorkplacesForm(channelBuilder, currentUser));
        }

        private void usersMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<UsersForm>(() => new UsersForm(channelBuilder, currentUser));
        }

        private void servicesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ServicesForm>(() => new ServicesForm(channelBuilder, currentUser));
        }

        private void clientsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientsForm>(() => new ClientsForm(channelBuilder, currentUser));
        }

        private void addClientRequestMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<AddClentRequestForm>(() => new AddClentRequestForm(channelBuilder, currentUser));
        }

        private void clientRequestsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ClientRequestsForm>(() => new ClientRequestsForm(channelBuilder, currentUser));
        }

        private void queueMonitorMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<QueueMonitorForm>(() => new QueueMonitorForm(channelBuilder, currentUser));
        }

        private void сurrentScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<CurrentScheduleForm>(() => new CurrentScheduleForm(channelBuilder, currentUser));
        }

        private void serviceRatingReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ServiceRatingReportForm>(() => new ServiceRatingReportForm(channelBuilder, currentUser));
        }

        private void operatorsRatingToolStripMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<OperatorRatingReportForm>(() => new OperatorRatingReportForm(channelBuilder, currentUser));
        }

        private void exceptionScheduleReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ShowForm<ExceptionScheduleReportForm>(() => new ExceptionScheduleReportForm(channelBuilder, currentUser));
        }

        private void officesMenuItem_Click(object sender, EventArgs eventArgsventArgs)
        {
            ShowForm<OfficesForm>(() => new OfficesForm(channelBuilder, currentUser));
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
    }
}