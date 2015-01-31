using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms.Controls
{
    public partial class ServerConnectionSettingsControl : UserControl
    {
        private const string SectionKey = "connection";

        private UserRole userRole;
        private TaskPool taskPool;
        private IConfigurationManager configuration;

        public ChannelManager<IServerTcpService> ChannelManager { get; private set; }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        public User SelectedUSer
        {
            get { return selectUserControl.Selected<User>(); }
        }

        public EventHandler OnConnected = delegate { };

        public EventHandler OnSubmit = delegate { };

        public ServerConnectionSettings ConnectionSettings { get; private set; }

        public ServerConnectionSettingsControl()
        {
            InitializeComponent();
        }

        public void Initialize(UserRole userRole, TaskPool taskPool)
        {
            this.userRole = userRole;
            this.taskPool = taskPool;

            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            ConnectionSettings = configuration.GetSection<ServerConnectionSettings>(SectionKey, (s) => s.Endpoint = "net.tcp://queue:4505");

            serverConnectionSettingsBindingSource.DataSource = ConnectionSettings;

            if (ConnectionSettings.User != Guid.Empty)
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
                if (string.IsNullOrWhiteSpace(ConnectionSettings.Endpoint))
                {
                    throw new QueueException("Не указан адрес сервера");
                }

                if (ChannelBuilder != null)
                {
                    ChannelBuilder.Dispose();
                }

                ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(),
                                                                            Bindings.NetTcpBinding,
                                                                            new EndpointAddress(ConnectionSettings.Endpoint));
                if (ChannelManager != null)
                {
                    ChannelManager.Dispose();
                }

                ChannelManager = new ChannelManager<IServerTcpService>(ChannelBuilder);

                using (Channel<IServerTcpService> channel = ChannelManager.CreateChannel())
                {
                    connectButton.Enabled = false;

                    selectUserControl.Initialize(await taskPool.AddTask(channel.Service.GetUserLinks(userRole)));
                    if (ConnectionSettings.User != Guid.Empty)
                    {
                        selectUserControl.Select<User>(new User() { Id = ConnectionSettings.User });
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
            if (ConnectionSettings != null)
            {
                ConnectionSettings.User = selectUserControl.Selected<User>().Id;
            }
        }

        internal void Close()
        {
            if (ChannelManager != null)
            {
                ChannelManager.Dispose();
            }
        }

        public static void ResetSettings()
        {
            IConfigurationManager configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            ServerConnectionSettings connection = configuration.GetSection<ServerConnectionSettings>(SectionKey);
            connection.Password = string.Empty;

            configuration.Save();
        }
    }
}