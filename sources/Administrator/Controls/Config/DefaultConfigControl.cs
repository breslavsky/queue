using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class DefaultConfigControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private DefaultConfig config;

        #endregion fields

        #region properties

        public DefaultConfig Config
        {
            set
            {
                config = value;
                if (config != null)
                {
                    queueNameTextBox.Text = config.QueueName;
                    workStartTimeTextBox.Text = config.WorkStartTime.ToString("hh\\:mm");
                    workFinishTimeTextBox.Text = config.WorkFinishTime.ToString("hh\\:mm");
                    maxClientRequestsUpDown.Value = config.MaxClientRequests;
                    maxRenderingTimeUpDown.Value = config.MaxRenderingTime;
                }
            }
        }

        #endregion properties

        public DefaultConfigControl()
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

        private void queueNameTextBox_Leave(object sender, EventArgs e)
        {
            config.QueueName = queueNameTextBox.Text;
        }

        private void maxClientRequestsUpDown_Leave(object sender, EventArgs e)
        {
            config.MaxClientRequests = (int)maxClientRequestsUpDown.Value;
        }

        private void maxRenderingTimeUpDown_Leave(object sender, EventArgs e)
        {
            config.MaxRenderingTime = (int)maxRenderingTimeUpDown.Value;
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
    }
}