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
        private UserRole userRole;
        private LoginSettings loginSettings;

        private IUnityContainer container;
        private IServerServiceManager serviceManager;
        private ChannelManager<IServerTcpService> channelManager;

        public User SelectedUser
        {
            get { return userControl.Selected<User>(); }
        }

        public EventHandler OnConnected = delegate { };
        public EventHandler OnSubmit = delegate { };

        public LoginSettingsControl()
        {
            InitializeComponent();

            container = ServiceLocator.Current.GetInstance<IUnityContainer>();
        }

        public void Initialize(LoginSettings loginSettings, UserRole userRole)
        {
            this.userRole = userRole;
            this.loginSettings = loginSettings;

            serverConnectionSettingsBindingSource.DataSource = loginSettings;

            if (loginSettings.User != Guid.Empty)
            {
                ConnectToServer();
            }
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
                if (string.IsNullOrWhiteSpace(loginSettings.Endpoint))
                {
                    throw new QueueException("Не указан адрес сервера");
                }

                if (serviceManager != null)
                {
                    serviceManager.Dispose();
                }

                serviceManager = new ServerServiceManager(Schemes.NET_TCP, hostTextBox.Text, (int)portUpDown.Value);

                if (channelManager != null)
                {
                    channelManager.Dispose();
                }

                channelManager = serviceManager.Server.CreateChannelManager();

                using (var channel = channelManager.CreateChannel())
                {
                    connectButton.Enabled = false;

                    userControl.Initialize(await channel.Service.GetUserLinks(userRole));
                    if (loginSettings.User != Guid.Empty)
                    {
                        userControl.Select<User>(new User() { Id = loginSettings.User });
                    }

                    container.RegisterInstance<IServerServiceManager>(serviceManager);

                    passwordTextBox.Focus();

                    connected = true;

                    loginGroupBox.Enabled = true;
                }
            }
            catch (OperationCanceledException) { }
            catch (CommunicationObjectAbortedException) { }
            catch (ObjectDisposedException) { }
            //catch (InvalidOperationException) { }
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

        private void selectUserControl_SelectedChanged(object sender, EventArgs e)
        {
            loginSettings.User = userControl.Selected<User>().Id;
        }

        internal void Close()
        {
            if (channelManager != null)
            {
                channelManager.Dispose();
            }
        }
    }
}