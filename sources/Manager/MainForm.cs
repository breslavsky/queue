using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Common;
using Queue.Services.Contracts;
using System;
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using QIcons = Queue.UI.Common.Icons;
using QueueManager = Queue.Services.DTO.Manager;
using Timer = System.Timers.Timer;

namespace Queue.Manager
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private QueueManager currentManager;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private Channel<IServerTcpService> pingChannel;

        private Timer pingTimer;
        private int PING_INTERVAL = 10000;

        public bool IsLogout { get; private set; }

        public MainForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, QueueManager currentManager)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentManager = currentManager;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();

            Text = currentManager.ToString();

            pingChannel = channelManager.CreateChannel();

            pingTimer = new Timer();
            pingTimer.Elapsed += pingTimer_Elapsed;
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
                Invoke(new MethodInvoker(async () =>
                {
                    try
                    {
                        serverStateLabel.Image = QIcons.connecting16x16;

                        if (!pingChannel.IsConnected)
                        {
                            await taskPool.AddTask(pingChannel.Service.OpenUserSession(currentManager.SessionId));
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
                }));
            }
            catch
            {
                // nothing
            }
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    await taskPool.AddTask(channel.Service.OpenUserSession(currentManager.SessionId));
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

        private Form GetChildForm(Type formType)
        {
            foreach (var form in MdiChildren)
            {
                if (form.GetType() == formType)
                {
                    return form;
                }
            }

            return null;
        }

        private void clientsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ClientsForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ClientsForm(channelBuilder, currentManager)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void addClientRequestMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(AddClentRequestForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new AddClentRequestForm(channelBuilder, currentManager)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void clientRequestsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ClientRequestsForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ClientRequestsForm(channelBuilder, currentManager)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void queueMonitorMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(QueueMonitorForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new QueueMonitorForm(channelBuilder, currentManager)
            {
                MdiParent = this
            };
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