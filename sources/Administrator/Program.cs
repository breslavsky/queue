using CommandLine;
using Junte.Configuration;
using Junte.UI.WinForms;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.ServiceModel;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Administrator
{
    internal static class Program
    {
        private const string AppName = "Queue.Administrator";
        private static AppOptions options;
        private static UnityContainer container;
        private static IConfigurationManager configuration;
        private static LoginSettings loginSettings;
        private static LoginFormSettings loginFormSettings;
        private static IClientService<IServerTcpService> serverService;
        private static QueueAdministrator currentUser;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            container = new UnityContainer();
            configuration = new ConfigurationManager(AppName, SpecialFolder.ApplicationData);
            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);

            RegisterContainer();
            ParseOptions();

            if (options.AutoLogin)
            {
                serverService = new ClientService<IServerTcpService>(options.Endpoint, ServerServicesPaths.Server);

                var channelManager = serverService.CreateChannelManager();
                using (var channel = channelManager.CreateChannel())
                {
                    Guid sessionId = Guid.Parse(options.SessionId);
                    currentUser = channel.Service.OpenUserSession(sessionId).Result as QueueAdministrator;
                    container.RegisterInstance<IClientService<IServerTcpService>>(serverService);
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
                        currentUser = loginForm.CurrentUser as QueueAdministrator;
                        container.RegisterInstance<QueueAdministrator>(currentUser);

                        loginForm.Dispose();

                        serverService = new ClientService<IServerTcpService>(loginSettings.Endpoint, ServerServicesPaths.Server);
                        container.RegisterInstance<IClientService<IServerTcpService>>(serverService);

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

            serverService.Dispose();
        }

        private static void ParseOptions()
        {
            options = new AppOptions();
            CommandLine.Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), options);
        }

        private static void RegisterContainer()
        {
            container.RegisterInstance<IConfigurationManager>(configuration);
            container.RegisterInstance<LoginSettings>(loginSettings);
            container.RegisterInstance<LoginFormSettings>(loginFormSettings);

            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        private static void ResetSettings()
        {
            var configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();

            configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey).Reset();
            configuration.GetSection<LoginSettings>(LoginSettings.SectionKey).Reset();

            configuration.Save();
        }
    }
}