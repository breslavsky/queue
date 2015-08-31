using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class SMTPConfigControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        public IClientService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private SMTPConfig config;

        #endregion fields

        #region properties

        public SMTPConfig Config
        {
            get
            {
                return config;
            }
            private set
            {
                config = value;
                if (config != null)
                {
                    serverTextBox.Text = config.Server;
                    portUpDown.Value = config.Port;
                    enableSslCheckBox.Checked = config.EnableSsl;
                    userTextBox.Text = config.User;
                    passwordTextBox.Text = config.Password;
                    fromTextBox.Text = config.From;
                }
            }
        }

        #endregion properties

        public SMTPConfigControl()
        {
            InitializeComponent();

            config = new SMTPConfig();

            if (designtime)
            {
                return;
            }

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void enableSslCheckBox_Leave(object sender, EventArgs e)
        {
            config.EnableSsl = enableSslCheckBox.Checked;
        }

        private void fromTextBox_Leave(object sender, EventArgs e)
        {
            config.From = fromTextBox.Text;
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            config.Password = passwordTextBox.Text;
        }

        private void portUpDown_Leave(object sender, EventArgs e)
        {
            config.Port = (int)portUpDown.Value;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditSMTPConfig(config));
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
                finally
                {
                    saveButton.Enabled = true;
                }
            }
        }

        private void serverTextBox_Leave(object sender, EventArgs e)
        {
            config.Server = serverTextBox.Text;
        }

        private async void SMTPConfigControl_Load(object sender, EventArgs e)
        {
            if (designtime)
            {
                return;
            }

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Config = await taskPool.AddTask(channel.Service.GetSMTPConfig());
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void userTextBox_Leave(object sender, EventArgs e)
        {
            config.User = userTextBox.Text;
        }
    }
}