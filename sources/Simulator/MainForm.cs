using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Simulator
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private TaskPool taskPool;

        public MainForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
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