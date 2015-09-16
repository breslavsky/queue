using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Reflection;
using System.ServiceModel;

namespace Queue.Simulator
{
    public partial class MainForm : RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private DuplexChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public MainForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new DuplexChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();

            Text = currentUser.ToString();
        }

        public bool IsLogout { get; private set; }

        private void clientRequestsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = new ClientRequstsForm(channelBuilder, currentUser)
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

        private void MainForm_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            taskPool.Dispose();
            channelManager.Dispose();
        }

        private async void MainForm_Load(object sender, EventArgs eventArgs)
        {
            Text += string.Format(" ({0})", Assembly.GetEntryAssembly().GetName().Version);

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    DefaultConfig config = await taskPool.AddTask(channel.Service.GetDefaultConfig());
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
        }

        private void opeatorsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            var form = new OperatorsForm(channelBuilder, currentUser)
            {
                MdiParent = this
            };
            FormClosing += (s, e) =>
            {
                form.Close();
            };
            form.Show();
        }
    }
}