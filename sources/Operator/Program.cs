using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;
using QueueOperator = Queue.Services.DTO.Operator;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Operator
{
    internal static class Program
    {
        private const string AppName = "Queue.Operator";
        private static AppOptions options;
        private static UnityContainer container;
        private static IConfigurationManager configuration;
        private static LoginSettings loginSettings;
        private static LoginFormSettings loginFormSettings;
        private static IClientService<IServerTcpService> serverService;
        private static QueueOperator currentUser;

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
                    currentUser = channel.Service.OpenUserSession(sessionId).Result as QueueOperator;
                    container.RegisterInstance<IClientService<IServerTcpService>>(serverService);
                    container.RegisterInstance<User>(currentUser);
                    container.RegisterInstance<QueueOperator>(currentUser);

                    Application.Run(new OperatorForm());
                }
            }
            else
            {
                while (true)
                {
                    var loginForm = new LoginForm(UserRole.Operator);
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        currentUser = loginForm.CurrentUser as QueueOperator;
                        container.RegisterInstance<QueueOperator>(currentUser);

                        loginForm.Dispose();

                        serverService = new ClientService<IServerTcpService>(loginSettings.Endpoint, ServerServicesPaths.Server);
                        container.RegisterInstance<IClientService<IServerTcpService>>(serverService);

                        var mainForm = new OperatorForm();
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