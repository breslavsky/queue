﻿using Junte.Parallel.Common;
using Junte.UI.WinForms;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Globalization;
using System.ServiceModel;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class LoginForm : RichForm
    {
        private const string SectionKey = "login";

        private readonly TaskPool taskPool;
        private readonly UserRole userRole;
        private LoginFormSettings settings;
        private IApplicationConfigurationManager configuration;

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
            configuration = ServiceLocator.Current.GetInstance<IApplicationConfigurationManager>();
            settings = configuration.GetSection<LoginFormSettings>(SectionKey);

            loginFormSettingsBindingSource.DataSource = settings;

            languageControl.Select<Language>(CultureInfo.CurrentCulture.GetLanguage());
            serverConnectionSettingsControl.Initialize(userRole, taskPool);
            serverConnectionSettingsControl.OnConnected += OnConnected;
            serverConnectionSettingsControl.OnSubmit += OnSubmit;

            if (settings.IsRemember)
            {
                serverConnectionSettingsControl.Connect();
            }
        }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder
        {
            get { return serverConnectionSettingsControl.ChannelBuilder; }
        }

        public ServerConnectionSettings ConnectionSettings
        {
            get { return serverConnectionSettingsControl.ConnectionSettings; }
        }

        public User User { get; private set; }

        private void languageControl_SelectedChanged(object sender, EventArgs e)
        {
            Language language = languageControl.Selected<Language>();
            language.SetCurrent();

            Translate();
        }

        private async void Login()
        {
            User selectedUser = serverConnectionSettingsControl.SelectedUSer;
            if (selectedUser == null)
            {
                return;
            }

            using (Channel<IServerTcpService> channel = serverConnectionSettingsControl.ChannelManager.CreateChannel())
            {
                try
                {
                    loginButton.Enabled = false;

                    User = await taskPool.AddTask(channel.Service.UserLogin(selectedUser.Id, ConnectionSettings.Password));

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
            serverConnectionSettingsControl.Close();
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

        public static void ResetSettings()
        {
            IApplicationConfigurationManager configuration = ServiceLocator.Current.GetInstance<IApplicationConfigurationManager>();
            LoginFormSettings settings = configuration.GetSection<LoginFormSettings>(SectionKey);
            settings.IsRemember = false;

            configuration.Save();
        }
    }
}