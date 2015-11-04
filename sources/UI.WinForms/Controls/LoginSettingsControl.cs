using Junte.UI.WinForms;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginSettingsControl : UserControl
    {
        private LoginSettings settings;

        #region properties

        public UserRole UserRole { get; set; }

        public LoginSettings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                settingsBindingSource.DataSource = value;
            }
        }

        #endregion properties

        #region events

        public EventHandler OnConnected = delegate { };
        public EventHandler OnSubmit = delegate { };

        #endregion events

        public LoginSettingsControl()
        {
            InitializeComponent();

            Settings = new LoginSettings();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            ConnectToServer();
        }

        private void ConnectToServer()
        {
            try
            {
                Connect();
            }
            catch (Exception ex)
            {
                UIHelper.Warning(ex.Message);
            }
        }

        public async void Connect()
        {
            bool connected = false;

            try
            {
                if (string.IsNullOrWhiteSpace(Settings.Endpoint))
                {
                    throw new QueueException("Не указан адрес сервера");
                }

                using (var serverService = new ServerUserService(Settings.Endpoint))
                using (var channelManager = serverService.CreateChannelManager())
                using (var channel = channelManager.CreateChannel())
                {
                    connectButton.Enabled = false;

                    userControl.Initialize(await channel.Service.GetUserLinks(UserRole));
                    if (Settings.User != Guid.Empty)
                    {
                        userControl.Select<User>(new User() { Id = Settings.User });
                    }

                    passwordTextBox.Focus();

                    connected = true;

                    connectionGroupBox.Enabled = false;
                    loginGroupBox.Enabled = true;
                }
            }
            catch (OperationCanceledException) { }
            catch (CommunicationObjectAbortedException) { }
            catch (ObjectDisposedException) { }
            catch (InvalidOperationException) { }
            catch (FaultException ex)
            {
                UIHelper.Warning(ex.Reason.ToString());
            }
            catch (Exception ex)
            {
                UIHelper.Warning(ex.Message);
            }
            finally
            {
                connectButton.Enabled = true;
            }

            if (connected)
            {
                OnConnected(this, null);
            }
        }

        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            passwordTextBox.SelectAll();
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                OnSubmit(this, null);
            }
        }

        private void endpointTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Connect();
            }
        }

        private void userControl_SelectedChanged(object sender, EventArgs e)
        {
            Settings.User = userControl.Selected<User>().Id;
        }
    }
}