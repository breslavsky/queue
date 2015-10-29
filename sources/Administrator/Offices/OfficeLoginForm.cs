using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class OfficeLoginForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        #endregion dependency

        #region fields

        private readonly Guid officeId;
        private readonly TaskPool taskPool;
        private Office office;

        #endregion fields

        #region properties

        public Office Office
        {
            get { return office; }
            private set
            {
                office = value;
            }
        }

        public Guid SessionId { get; private set; }

        public LoginSettings Settings { get; private set; }

        #endregion properties

        public OfficeLoginForm(Guid officeId)
        {
            InitializeComponent();

            Settings = new LoginSettings();
            loginSettingsControl.Settings = Settings;

            this.officeId = officeId;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
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
                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private async void loginButton_Click(object sender, EventArgs e)
        {
            using (var serverService = new ServerService(Settings.Endpoint, ServerServicesPaths.Server))
            using (var channelManager = serverService.CreateChannelManager())
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    loginButton.Enabled = false;

                    var user = await taskPool.AddTask(channel.Service.UserLogin(Settings.User, Settings.Password));
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

        private void OfficeLoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Cancel();
        }

        private async void OfficeLoginForm_Load(object sender, EventArgs e)
        {
            Enabled = false;

            using (var channel = ChannelManager.CreateChannel())
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }
    }
}