using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public IConfigurationManager Configuration { get; set; }

        [Dependency]
        public LoginFormSettings LoginFormSettings { get; set; }

        [Dependency]
        public LoginSettings LoginSettings { get; set; }

        #endregion dependency

        #region properties

        public User CurrentUser { get; private set; }

        #endregion properties

        #region fields

        private IClientService<IServerTcpService> serverService;
        private ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private readonly UserRole userRole;

        #endregion fields

        public LoginForm(UserRole userRole)
            : base()
        {
            InitializeComponent();

            this.userRole = userRole;

            taskPool = new TaskPool();
            taskPool.OnAddTask += taskPool_OnAddTask;
            taskPool.OnRemoveTask += taskPool_OnRemoveTask;

            languageControl.Initialize<Language>();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", Assembly.GetEntryAssembly().GetName().Version);

            loginFormSettingsBindingSource.DataSource = LoginFormSettings;

            languageControl.Select(LoginFormSettings.Language);
            AdjustSelectedLanguage();

            loginSettingsControl.UserRole = userRole;
            loginSettingsControl.OnConnected += OnConnected;
            loginSettingsControl.OnSubmit += OnSubmit;

            if (LoginFormSettings.IsRemember)
            {
                loginSettingsControl.Connect();
            }
        }

        private void languageControl_SelectedChanged(object sender, EventArgs e)
        {
            AdjustSelectedLanguage();
        }

        private void AdjustSelectedLanguage()
        {
            var language = languageControl.Selected<Language>();
            language.SetCurrent();

            Translate();
        }

        private async void Login()
        {
            if (serverService != null)
            {
                serverService.Dispose();
            }

            serverService = new ClientService<IServerTcpService>(LoginSettings.Endpoint);

            if (channelManager != null)
            {
                channelManager.Dispose();
            }

            channelManager = serverService.CreateChannelManager();

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    loginButton.Enabled = false;

                    CurrentUser = await taskPool.AddTask(channel.Service.UserLogin(LoginSettings.User, LoginSettings.Password));

                    if (!LoginFormSettings.IsRemember)
                    {
                        LoginSettings.Password = string.Empty;
                    }

                    LoginFormSettings.Language = languageControl.Selected<Language>();

                    Configuration.Save();
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
                }
            }
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            Login();
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            taskPool.Dispose();

            if (serverService != null)
            {
                serverService.Dispose();
            }

            if (channelManager != null)
            {
                channelManager.Dispose();
            }
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            Login();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            if (LoginFormSettings.IsRemember)
            {
                Login();
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
    }
}