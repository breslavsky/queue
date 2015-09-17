using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Administrator.Settings;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Administrator
{
    internal static class Program
    {
        private static AppOptions options;
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static ApplicationSettings applicationSettings;
        private static AdministratorSettings administratorSettings;
        private static LoginSettings loginSettings;
        private static LoginFormSettings loginFormSettings;
        private static ServerService serverService;
        private static QueueAdministrator currentUser;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(Product.Administrator.AppName, SpecialFolder.ApplicationData);
            container.RegisterInstance(configuration);

            applicationSettings = configuration.GetSection<ApplicationSettings>(ApplicationSettings.SectionKey);
            container.RegisterInstance(applicationSettings);

            administratorSettings = configuration.GetSection<AdministratorSettings>(AdministratorSettings.SectionKey);
            container.RegisterInstance(administratorSettings);

            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            container.RegisterInstance(loginSettings);

            loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
            container.RegisterInstance(loginFormSettings);

            ParseOptions();

            if (options.AutoLogin)
            {
                serverService = new ServerService(options.Endpoint, ServerServicesPaths.Server);
                container.RegisterInstance(serverService);

                var channelManager = serverService.CreateChannelManager();
                using (var channel = channelManager.CreateChannel())
                {
                    Guid sessionId = Guid.Parse(options.SessionId);
                    currentUser = channel.Service.OpenUserSession(sessionId).Result as QueueAdministrator;
                    container.RegisterInstance<User>(currentUser);
                    container.RegisterInstance<QueueAdministrator>(currentUser);

                    Application.Run(new AdministratorForm());
                }
            }
            else
            {
                while (true)
                {
                    var loginForm = new LoginForm(UserRole.Administrator);
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        configuration.Save();

                        currentUser = loginForm.CurrentUser as QueueAdministrator;
                        container.RegisterInstance<User>(currentUser);
                        container.RegisterInstance<QueueAdministrator>(currentUser);

                        loginForm.Dispose();

                        serverService = new ServerService(loginSettings.Endpoint, ServerServicesPaths.Server);
                        container.RegisterInstance(serverService);

                        var mainForm = new AdministratorForm();
                        Application.Run(mainForm);

                        if (mainForm.IsLogout)
                        {
                            ResetSettings();
                            continue;
                        }
                    }

                    break;
                }
            }

            if (serverService != null)
            {
                serverService.Dispose();
            }
        }

        private static void ParseOptions()
        {
            options = new AppOptions();
            CommandLine.Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), options);
        }

        private static void ResetSettings()
        {
            loginFormSettings.Reset();
            loginSettings.Reset();

            configuration.Save();
        }
    }
}