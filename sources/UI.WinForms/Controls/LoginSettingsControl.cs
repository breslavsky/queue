using Junte.UI.WinForms;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
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
    public partial class LoginSettingsControl : DependencyUserControl
    {
        #region properties

        public UserRole UserRole { get; set; }

        public LoginSettings Settings { get; set; }

        #endregion properties

        #region events

        public EventHandler OnConnected = delegate { };
        public EventHandler OnSubmit = delegate { };

        #endregion events

        public LoginSettingsControl()
        {
            InitializeComponent();

            Settings = new LoginSettings();

            if (designtime)
            {
                return;
            }

            ServiceLocator.Current.GetInstance<UnityContainer>().BuildUp(this);
        }

        private void LoginSettingsControl_Load(object sender, EventArgs e)
        {
            settingsBindingSource.DataSource = Settings;
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
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
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

                using (var serverService = new ClientService<IServerTcpService>(Settings.Endpoint))
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

                    loginGroupBox.Enabled = true;
                }
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