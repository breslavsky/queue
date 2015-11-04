using Junte.Configuration;
using Junte.WCF;
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
        private static HubSettings hubSettings;
        private static LoginSettings loginSettings;
        private static LoginFormSettings loginFormSettings;

        private static string endpoint;
        private static Guid sessionId;
        private static QueueOperator currentUser;

        private static ServerService serverService;
        private static ServerUserService serverUserService;
        private static HubQualityService hubQualityService;

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

            hubSettings = configuration.GetSection<HubSettings>(HubSettings.SectionKey);
            container.RegisterInstance(hubSettings);

            ParseOptions();

            if (options.AutoLogin)
            {
                using (var serverUserService = new ServerUserService(options.Endpoint))
                using (var channelManager = serverUserService.CreateChannelManager())
                using (var channel = channelManager.CreateChannel())
                {
                    sessionId = Guid.Parse(options.SessionId);
                    currentUser = channel.Service.OpenUserSession(sessionId).Result as QueueOperator;
                }

                container.RegisterInstance<User>(currentUser);
                container.RegisterInstance<QueueOperator>(currentUser);

                RegisterServices();

                Application.Run(new OperatorForm());
            }
            else
            {
                while (true)
                {
                    var loginForm = new LoginForm(UserRole.Operator);
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        configuration.Save();

                        endpoint = loginSettings.Endpoint;
                        currentUser = loginForm.CurrentUser as QueueOperator;
                        sessionId = currentUser.SessionId;

                        container.RegisterInstance<User>(currentUser);
                        container.RegisterInstance<QueueOperator>(currentUser);

                        RegisterServices();

                        loginForm.Dispose();

                        using (var f = new OperatorForm())
                        {
                            Application.Run(f);

                            if (f.IsLogout)
                            {
                                ResetSettings();
                                continue;
                            }
                        }
                    }

                    break;
                }
            }
        }

        private static void ParseOptions()
        {
            options = new AppOptions();
            CommandLine.Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), options);
        }

        private static void RegisterServices()
        {
            hubQualityService = new HubQualityService(hubSettings.Endpoint, HubServicesPaths.Quality);
            container.RegisterInstance(hubQualityService);
            container.RegisterType<DuplexChannelManager<IHubQualityTcpService>>
                (new InjectionFactory(c => hubQualityService.CreateChannelManager()));

            serverService = new ServerService(endpoint);
            container.RegisterInstance(serverService);
            container.RegisterType<DuplexChannelManager<IServerTcpService>>
                (new InjectionFactory(c => serverService.CreateChannelManager(sessionId)));

            serverUserService = new ServerUserService(endpoint);
            container.RegisterInstance(serverUserService);
            container.RegisterType<ChannelManager<IServerUserTcpService>>
                (new InjectionFactory(c => serverUserService.CreateChannelManager(sessionId)));
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