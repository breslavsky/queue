using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Common.Settings;
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
        private static AppOptions options;
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static HubQualitySettings hubQualitySettings;
        private static LoginSettings loginSettings;
        private static LoginFormSettings loginFormSettings;
        private static ServerService<IServerTcpService> serverService;
        private static HubQualityService hubQualityService;
        private static QueueOperator currentUser;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(Product.Operator.AppName, SpecialFolder.ApplicationData);
            container.RegisterInstance(configuration);

            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            container.RegisterInstance(loginSettings);

            loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
            container.RegisterInstance(loginFormSettings);

            hubQualitySettings = configuration.GetSection<HubQualitySettings>(HubQualitySettings.SectionKey);
            container.RegisterInstance(hubQualitySettings);

            hubQualityService = new HubQualityService(hubQualitySettings.Endpoint, HubServicesPaths.Quality);
            container.RegisterInstance(hubQualityService);

            ParseOptions();

            if (options.AutoLogin)
            {
                serverService = new ServerService<IServerTcpService>(options.Endpoint, ServerServicesPaths.Server);

                var channelManager = serverService.CreateChannelManager();
                using (var channel = channelManager.CreateChannel())
                {
                    Guid sessionId = Guid.Parse(options.SessionId);
                    currentUser = channel.Service.OpenUserSession(sessionId).Result as QueueOperator;
                    container.RegisterInstance(serverService);
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
                        configuration.Save();

                        currentUser = loginForm.CurrentUser as QueueOperator;
                        container.RegisterInstance<QueueOperator>(currentUser);

                        loginForm.Dispose();

                        serverService = new ServerService<IServerTcpService>(loginSettings.Endpoint, ServerServicesPaths.Server);
                        container.RegisterInstance<ServerService<IServerTcpService>>(serverService);

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

        private static void ResetSettings()
        {
            var configuration = ServiceLocator.Current.GetInstance<ConfigurationManager>();

            loginFormSettings.Reset();
            loginSettings.Reset();

            configuration.Save();
        }
    }
}