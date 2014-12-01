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
    public partial class PortalConfigControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;

        private PortalConfig config;

        #endregion fields

        #region properties

        public PortalConfig Config
        {
            set
            {
                config = value;
                if (config != null)
                {
                    headerTextBox.Text = config.Header;
                    footerTextBox.Text = config.Footer;
                    currentDayRecordingCheckBox.Checked = config.CurrentDayRecording;
                }
            }
        }

        #endregion properties

        public PortalConfigControl()
        {
            InitializeComponent();
        }

        public void Initialize(DuplexChannelBuilder<IServerTcpService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void headerTextBox_Click(object sender, EventArgs e)
        {
            using (var f = new HtmlEditorForm())
            {
                f.HTML = headerTextBox.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    headerTextBox.Text = f.HTML;
                }
            }
        }

        private void headerTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Header = headerTextBox.Text;
            }
        }

        private void footerTextBox_Click(object sender, EventArgs e)
        {
            using (var f = new HtmlEditorForm())
            {
                f.HTML = footerTextBox.Text;
                if (f.ShowDialog() == DialogResult.OK)
                {
                    footerTextBox.Text = f.HTML;
                }
            }
        }

        private void footerTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Footer = footerTextBox.Text;
            }
        }

        private void portalCurrentDayRecordingCheckBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.CurrentDayRecording = currentDayRecordingCheckBox.Checked;
            }
        }

        private async void saveButton_Click(object sender, EventArgs e)
        {
            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    saveButton.Enabled = false;

                    await taskPool.AddTask(channel.Service.OpenUserSession(currentUser.SessionId));
                    await taskPool.AddTask(channel.Service.EditPortalConfig(config));
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