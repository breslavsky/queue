using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.Administrator
{
    public partial class TerminalConfigControl : RichUserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private TerminalConfig config;

        #endregion fields

        #region properties

        public TerminalConfig Config
        {
            set
            {
                config = value;
                if (config != null)
                {
                    PINUpDown.Value = config.PIN;
                    currentDayRecordingCheckBox.Checked = config.CurrentDayRecording;
                    columnsUpDown.Value = config.Columns;
                    rowsUpDown.Value = config.Rows;
                }
            }
        }

        #endregion properties

        public TerminalConfigControl()
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

        private void PINUpDown_Leave(object sender, EventArgs e)
        {
            config.PIN = (int)PINUpDown.Value;
        }

        private void columnsUpDown_Leave(object sender, EventArgs e)
        {
            config.Columns = (int)columnsUpDown.Value;
        }

        private void rowsUpDown_Leave(object sender, EventArgs e)
        {
            config.Rows = (int)rowsUpDown.Value;
        }

        private void currentDayRecordingCheckBox_Leave(object sender, EventArgs e)
        {
            config.CurrentDayRecording = currentDayRecordingCheckBox.Checked;
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    config = await taskPool.AddTask(channel.Service.EditTerminalConfig(config));
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