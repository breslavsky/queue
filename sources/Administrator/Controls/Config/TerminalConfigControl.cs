using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Resources;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ComponentModel;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    public partial class TerminalConfigControl : DependencyUserControl
    {
        #region dependency

        [Dependency]
        [ReadOnly(true)]
        [Browsable(false)]
        public QueueAdministrator CurrentUser { get; set; }

        [Dependency]
        [ReadOnly(true)]
        [Browsable(false)]
        public ServerService<IServerTcpService> ServerService { get; set; }

        #endregion dependency

        #region fields

        private const string HighligtingStyle = "XML";
        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private TerminalConfig config;

        #endregion fields

        #region properties

        public TerminalConfig Config
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
                    PINUpDown.Value = config.PIN;
                    currentDayRecordingCheckBox.Checked = config.CurrentDayRecording;
                    columnsUpDown.Value = config.Columns;
                    rowsUpDown.Value = config.Rows;

                    windowTemplateEditor.Text = config.WindowTemplate;
                    windowTemplateEditor.SetHighlighting(HighligtingStyle);
                }
            }
        }

        #endregion properties

        public TerminalConfigControl()
        {
            InitializeComponent();

            config = new TerminalConfig();

            if (designtime)
            {
                return;
            }

            channelManager = ServerService.CreateChannelManager(CurrentUser.SessionId);

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;
        }

        private void columnsUpDown_Leave(object sender, EventArgs e)
        {
            config.Columns = (int)columnsUpDown.Value;
        }

        private void currentDayRecordingCheckBox_Leave(object sender, EventArgs e)
        {
            config.CurrentDayRecording = currentDayRecordingCheckBox.Checked;
        }

        private void PINUpDown_Leave(object sender, EventArgs e)
        {
            config.PIN = (int)PINUpDown.Value;
        }

        private void rowsUpDown_Leave(object sender, EventArgs e)
        {
            config.Rows = (int)rowsUpDown.Value;
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

        private void taskPool_OnAddTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.WaitCursor));
        }

        private void taskPool_OnRemoveTask(object sender, EventArgs e)
        {
            Invoke((MethodInvoker)(() => Cursor = Cursors.Default));
        }

        private void templateLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            windowTemplateEditor.Text = Templates.TerminalWindow;
        }

        private async void TerminalConfigControl_Load(object sender, EventArgs e)
        {
            if (designtime)
            {
                return;
            }

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Config = await taskPool.AddTask(channel.Service.GetTerminalConfig());
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

        private void windowTemplateEditor_Leave(object sender, EventArgs e)
        {
            config.WindowTemplate = windowTemplateEditor.Text;
        }
    }
}