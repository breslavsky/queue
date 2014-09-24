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
    public partial class SMTPConfigControl : UserControl
    {
        #region fields

        private DuplexChannelBuilder<IServerService> channelBuilder;
        private User currentUser;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private SMTPConfig config;

        #endregion fields

        #region properties

        public SMTPConfig Config
        {
            set
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
        }

        public void Initialize(DuplexChannelBuilder<IServerService> channelBuilder, User currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;

            channelManager = new ChannelManager<IServerService>(channelBuilder);
            taskPool = new TaskPool();
        }

        private void serverTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Server = serverTextBox.Text;
            }
        }

        private void portUpDown_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Port = (int)portUpDown.Value;
            }
        }

        private void enableSslCheckBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.EnableSsl = enableSslCheckBox.Checked;
            }
        }

        private void userTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.User = userTextBox.Text;
            }
        }

        private void passwordTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.Password = passwordTextBox.Text;
            }
        }

        private void fromTextBox_Leave(object sender, EventArgs e)
        {
            if (config != null)
            {
                config.From = fromTextBox.Text;
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
                    await taskPool.AddTask(channel.Service.EditSMTPConfig(config));
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