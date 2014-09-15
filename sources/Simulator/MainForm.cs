using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.DTO;
using Queue.Services.Contracts;
using System;
using System.ServiceModel;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Simulator
{
    public partial class MainForm : Queue.UI.WinForms.RichForm
    {
        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        public bool IsLogout { get; private set; }

        public MainForm(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
            : base()
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();

            Text = currentUser.ToString();
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
    }
}