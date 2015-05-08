using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class OfficeLoginForm : RichForm
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private ChannelManager<IServerTcpService> channelManager;
        private User currentUser;
        private Office office;
        private Guid officeId;
        private TaskPool taskPool;

        private LoginSettings loginSettings;

        public string Endpoint { get { return loginSettings.Endpoint; } }

        public Guid SessionId { get; private set; }

        public Office Office
        {
            get { return office; }
            private set
            {
                office = value;

                loginSettings = new LoginSettings()
                {
                    Endpoint = office.Endpoint
                };
                loginSettingsControl.Initialize(loginSettings, UserRole.Administrator);
            }
        }

        public OfficeLoginForm(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser, Guid officeId)
        {
            InitializeComponent();

            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.officeId = officeId;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder, currentUser.SessionId);
            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private async void OfficeLoginForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Office = await taskPool.AddTask(channel.Service.GetOffice(officeId));

                    Enabled = true;
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

        private void OfficeLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            User selectedUser = loginSettingsControl.SelectedUser;
            if (selectedUser == null)
            {
                return;
            }

            using (Channel<IServerTcpService> channel = loginSettingsControl.ChannelManager.CreateChannel())
            {
                try
                {
                    loginButton.Enabled = false;

                    var user = await taskPool.AddTask(channel.Service.UserLogin(selectedUser.Id, loginSettings.Password));
                    SessionId = user.SessionId;
                    DialogResult = DialogResult.OK;
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
                    loginButton.Enabled = true;
                }
            }
        }
    }
}