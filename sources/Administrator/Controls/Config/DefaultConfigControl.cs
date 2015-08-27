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
    public partial class DefaultConfigControl : DependencyUserControl
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
        private DefaultConfig config;

        #endregion fields

        #region properties

        public DefaultConfig Config
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
                    queueNameTextBox.Text = config.QueueName;
                    workStartTimeTextBox.Text = config.WorkStartTime.ToString("hh\\:mm");
                    workFinishTimeTextBox.Text = config.WorkFinishTime.ToString("hh\\:mm");
                    maxClientRequestsUpDown.Value = config.MaxClientRequests;
                    maxRenderingTimeUpDown.Value = config.MaxRenderingTime;
                    isDebugCheckBox.Checked = config.IsDebug;
                }
            }
        }

        #endregion properties

        public DefaultConfigControl()
        {
            InitializeComponent();

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private async void DefaultConfigControl_Load(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Config = await taskPool.AddTask(channel.Service.GetDefaultConfig());
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

        private void isDebugCheckBox_Leave(object sender, EventArgs e)
        {
            config.IsDebug = isDebugCheckBox.Checked;
        }

        private void maxClientRequestsUpDown_Leave(object sender, EventArgs e)
        {
            config.MaxClientRequests = (int)maxClientRequestsUpDown.Value;
        }

        private void maxRenderingTimeUpDown_Leave(object sender, EventArgs e)
        {
            config.MaxRenderingTime = (int)maxRenderingTimeUpDown.Value;
        }

        private void queueNameTextBox_Leave(object sender, EventArgs e)
        {
            config.QueueName = queueNameTextBox.Text;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditDefaultConfig(config));
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void workFinishTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                config.WorkFinishTime = TimeSpan.Parse(workFinishTimeTextBox.Text);
            }
            catch
            {
                UIHelper.Warning("Ошибочный формат времени окончания рабочего дня");
                return;
            }
        }

        private void workStartTimeTextBox_Leave(object sender, EventArgs e)
        {
            try
            {
                config.WorkStartTime = TimeSpan.Parse(workStartTimeTextBox.Text);
            }
            catch
            {
                UIHelper.Warning("Ошибочный формат времени начала рабочего дня");
                return;
            }
        }
    }
}