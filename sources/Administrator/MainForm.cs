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
using System.ServiceModel;
using System.Timers;
using System.Windows.Forms;
using QIcons = Queue.UI.Common.Icons;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using Timer = System.Timers.Timer;

namespace Queue.Administrator
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(MainForm));

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private Channel<IServerService> pingChannel;

        private Timer pingTimer;
        private int PING_INTERVAL = 10000;

        public MainForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
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

        private Form GetChildForm(Type formType)
        {
            foreach (Form form in MdiChildren)
            {
                if (form.GetType() == formType)
                {
                    return form;
                }
            }

            return null;
        }

        private void aboutMenuItem_Click(object sender, EventArgs e)
        {
            new AboutForm().ShowDialog();
        }

        private void configMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ConfigForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ConfigForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };

            form.Show();
        }

        private void defaultScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(DefaultScheduleForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new DefaultScheduleForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void workplacesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(WorkplacesForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new WorkplacesForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void usersMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(UsersForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new UsersForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void servicesMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ServicesForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ServicesForm(channelBuilder, currentUser)
            {
                MdiParent = this
            }; ;
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void clientsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ClientsForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ClientsForm(channelBuilder, currentUser)
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

            form = GetChildForm(typeof(AddClentRequestForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new AddClentRequestForm(channelBuilder, currentUser)
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

            form = GetChildForm(typeof(ClientRequestsForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ClientRequestsForm(channelBuilder, currentUser)
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

            form = new QueueMonitorForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void сurrentScheduleMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(CurrentScheduleForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new CurrentScheduleForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void serviceRatingReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ServiceRatingReportForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ServiceRatingReportForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void exceptionScheduleReportMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = GetChildForm(typeof(ExceptionScheduleReportForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new ExceptionScheduleReportForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }

        private void officesMenuItem_Click(object sender, EventArgs eventArgsventArgs)
        {
            var form = GetChildForm(typeof(OfficesForm));
            if (form != null)
            {
                form.Activate();
                return;
            }

            form = new OfficesForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
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
    }
}