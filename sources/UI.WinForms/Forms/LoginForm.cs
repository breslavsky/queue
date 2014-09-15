using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginForm : Queue.UI.WinForms.RichForm
    {
        private readonly UserRole userRole;

        private readonly TaskPool taskPool;

        private ChannelManager<IServerService> channelManager;

        public LoginForm(UserRole userRole)
            : base()
        {
            InitializeComponent();

            this.userRole = userRole;

            taskPool = new TaskPool();
        }

        public DuplexChannelBuilder<IServerService> ChannelBuilder { get; private set; }

        public string Endpoint
        {
            get { return endpointTextBox.Text; }
            set { endpointTextBox.Text = value; }
        }

        public Guid UserId { get; set; }

        public string Password
        {
            get { return passwordTextBox.Text; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    passwordTextBox.Text = value;
                }
            }
        }

        public bool IsRemember
        {
            get { return rememberCheckBox.Checked; }
            set { rememberCheckBox.Checked = value; }
        }

        public User User { get; private set; }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            if (IsRemember)
            {
                connect();
            }
        }

        private async void connect()
        {
            if (ChannelBuilder != null)
            {
                ChannelBuilder.Dispose();
            }

            try
            {
                ChannelBuilder = new DuplexChannelBuilder<IServerService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
                return;
            }

            if (channelManager != null)
            {
                channelManager.Dispose();
            }
            channelManager = new ChannelManager<IServerService>(ChannelBuilder);

            bool connected = false;

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    connectButton.Enabled = false;

                    usersComboBox.DisplayMember = DataListItem.Value;
                    usersComboBox.ValueMember = DataListItem.Key;
                    usersComboBox.DataSource = new BindingSource(await taskPool.AddTask(channel.Service.GetUserList(userRole)), null);
                    usersComboBox.SelectedIndex = 0;

                    if (UserId != Guid.Empty)
                    {
                        usersComboBox.SelectedValue = UserId;
                    }

                    passwordTextBox.Focus();

                    connected = true;

                    connectionGroupBox.Enabled = false;
                    loginGroupBox.Enabled = true;
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
            }

            if (connected && IsRemember)
            {
                login();
            }
        }

        private async void login()
        {
            if (usersComboBox.SelectedValue != null)
            {
                UserId = (Guid)usersComboBox.SelectedValue;

                using (var channel = channelManager.CreateChannel())
                {
                    try
                    {
                        loginButton.Enabled = false;

                        User = await taskPool.AddTask(channel.Service.UserLogin(UserId, passwordTextBox.Text));
                        DialogResult = DialogResult.OK;
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
                        loginButton.Enabled = true;
                        passwordTextBox.Focus();
                    }
                }
            }
        }

        private void usersComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            passwordTextBox.Focus();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            login();
        }

        private void passwordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }


        private void passwordTextBox_Enter(object sender, EventArgs e)
        {
            passwordTextBox.SelectAll();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            try
            {
                connect();
            }
            catch (Exception exception)
            {
                UIHelper.Warning(exception.Message);
            }
        }

        private void serverTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                    connect();
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                }
            }
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();
            if (channelManager != null)
            {
                channelManager.Dispose();
            }
        }
    }
}