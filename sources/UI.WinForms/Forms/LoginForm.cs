using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using System;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginForm : DependencyForm
    {
        #region dependency

        [Dependency]
        public ApplicationSettings ApplicationSettings { get; set; }

        [Dependency]
        public LoginFormSettings LoginFormSettings { get; set; }

        [Dependency]
        public LoginSettings LoginSettings { get; set; }

        #endregion dependency

        #region properties

        public User CurrentUser { get; private set; }

        #endregion properties

        #region fields

        private UserService serverUserService;
        private ChannelManager<IUserTcpService> channelManager;
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
            loginSettingsControl.Settings = LoginSettings;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            Text += string.Format(" ({0})", Assembly.GetEntryAssembly().GetName().Version);

            loginFormSettingsBindingSource.DataSource = LoginFormSettings;

            languageControl.Select(ApplicationSettings.Language);
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
            if (serverUserService != null)
            {
                serverUserService.Dispose();
            }

            serverUserService = new UserService(LoginSettings.Endpoint);

            if (channelManager != null)
            {
                channelManager.Dispose();
            }

            channelManager = serverUserService.CreateChannelManager();

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    loginButton.Enabled = false;

                    CurrentUser = await taskPool.AddTask(channel.Service.UserLogin(LoginSettings.User, LoginSettings.Password));

                    if (!LoginFormSettings.IsRemember)
                    {
                        LoginSettings.Password = string.Empty;
                    }

                    ApplicationSettings.Language = languageControl.Selected<Language>();

                    DialogResult = DialogResult.OK;
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

            if (serverUserService != null)
            {
                serverUserService.Dispose();
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