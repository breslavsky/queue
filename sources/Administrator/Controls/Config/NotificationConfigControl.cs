using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class NotificationConfigControl : RichUserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private NotificationConfig config;

        #endregion fields

        #region properties

        public NotificationConfig Config
        {
            set
            {
                config = value;
                if (config != null)
                {
                    clientRequestsLengthUpDown.Value = config.ClientRequestsLength;
                }
            }
        }

        #endregion properties

        public NotificationConfigControl()
        {
            InitializeComponent();
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

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

        private void clientRequestsLengthUpDown_Leave(object sender, EventArgs e)
        {
            config.ClientRequestsLength = (int)clientRequestsLengthUpDown.Value;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditNotificationConfig(config));
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

        private void NotificationConfigControl_Load(object sender, EventArgs e)
        {

        }
    }
}