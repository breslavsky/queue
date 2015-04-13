using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Reflection;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginForm : RichForm
    {
        private readonly TaskPool taskPool;
        private readonly UserRole userRole;
        private LoginFormSettings settings;
        private IConfigurationManager configuration;

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

            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            settings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
            LoginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);

            loginFormSettingsBindingSource.DataSource = settings;

            languageControl.Select<Language>(settings.Language);
            AdjustSelectedLanguage();

            loginSettingsControl.Initialize(LoginSettings, userRole, taskPool);
            loginSettingsControl.OnConnected += OnConnected;
            loginSettingsControl.OnSubmit += OnSubmit;

            if (settings.IsRemember)
            {
                loginSettingsControl.Connect();
            }
        }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder
        {
            get { return loginSettingsControl.ChannelBuilder; }
        }

        public LoginSettings LoginSettings { get; private set; }

        public User User { get; private set; }

        private void languageControl_SelectedChanged(object sender, EventArgs e)
        {
            AdjustSelectedLanguage();
        }

        private void AdjustSelectedLanguage()
        {
            Language language = languageControl.Selected<Language>();
            language.SetCurrent();

            Translate();
        }

        private async void Login()
        {
            User selectedUser = loginSettingsControl.SelectedUser;
            if (selectedUser == null)
            {
                return;
            }

            using (Channel<IServerTcpService> channel = loginSettingsControl.ChannelManager.CreateChannel())
            {
                try
                {
                    loginButton.Enabled = false;

                    User = await taskPool.AddTask(channel.Service.UserLogin(selectedUser.Id, LoginSettings.Password));

                    if (!settings.IsRemember)
                    {
                        LoginSettings.Password = string.Empty;
                    }

                    settings.Language = languageControl.Selected<Language>();

                    configuration.Save();
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
            loginSettingsControl.Close();
        }

        private void OnSubmit(object sender, EventArgs e)
        {
            Login();
        }

        private void OnConnected(object sender, EventArgs e)
        {
            if (settings.IsRemember)
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