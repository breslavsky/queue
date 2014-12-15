using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using log4net;
using Queue.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Linq;
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using QIcons = Queue.UI.Common.Icons;
using Timer = System.Timers.Timer;

namespace Queue.Administrator
{
    public partial class ManagerForm : Queue.UI.WinForms.RichForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(ManagerForm));

        private const int PingInterval = 10000;

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Channel<IServerTcpService> pingChannel;
        private Timer pingTimer;

        public bool IsLogout { get; private set; }

        public ManagerForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();

            pingChannel = channelManager.CreateChannel();

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;

            Text = currentUser.ToString();
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
                Invoke(new MethodInvoker(async () =>
                {
                    try
                    {
                        serverStateLabel.Image = QIcons.connecting16x16;

                        ServerDateTime.Sync(await taskPool.AddTask(pingChannel.Service.GetDateTime()));
                        currentDateTimeLabel.Text = ServerDateTime.Now.ToLongTimeString();

                        //TODO: think
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
                }));
            }
            catch (Exception exception)
            {
                logger.Error(exception);
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    var config = await taskPool.AddTask(channel.Service.GetDefaultConfig());
                    Text += string.Format(" | {0}", config.QueueName);

                    pingTimer.Start();
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
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

        private void logoutButton_Click(object sender, EventArgs e)
        {
            IsLogout = true;
            Close();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            pingTimer.Stop();
            taskPool.Dispose();
            channelManager.Dispose();
            pingChannel.Dispose();
        }
    }
}