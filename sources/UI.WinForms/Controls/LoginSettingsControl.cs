using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginSettingsControl : RichUserControl
    {
        #region dependency

        [Dependency]
        public LoginSettings Settings { get; set; }

        #endregion dependency

        public UserRole UserRole { get; set; }

        private ClientService<IServerTcpService> serverService;
        private ChannelManager<IServerTcpService> channelManager;

        public User SelectedUser
        {
            get { return userControl.Selected<User>(); }
        }

        public EventHandler OnConnected = delegate { };
        public EventHandler OnSubmit = delegate { };

        public LoginSettingsControl()
        {
            if (!DesignMode)
            {
                //ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);
                //settingsBindingSource.DataSource = Settings;
            }
            InitializeComponent();
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

                if (serverService != null)
                {
                    serverService.Dispose();
                }

                serverService = new ClientService<IServerTcpService>(Settings.Endpoint);

                if (channelManager != null)
                {
                    channelManager.Dispose();
                }

                channelManager = serverService.CreateChannelManager();

                using (var channel = channelManager.CreateChannel())
                {
                    connectButton.Enabled = false;

                    userControl.Initialize(await channel.Service.GetUserLinks(UserRole));
                    if (Settings.User != Guid.Empty)
                    {
                        userControl.Select<User>(new User() { Id = Settings.User });
                    }

                    //passwordTextBox.Focus();

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
            //passwordTextBox.SelectAll();
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

        private void selectUserControl_SelectedChanged(object sender, EventArgs e)
        {
            Settings.User = userControl.Selected<User>().Id;
        }
    }
}