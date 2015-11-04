using Junte.Configuration;
using Junte.WCF;
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

        private static string endpoint;
        private static Guid sessionId;
        private static QueueAdministrator currentUser;

        private static ServerService serverService;
        private static ServerTemplateService serverTemplateService;
        private static ServerUserService serverUserService;
        private static ServerWorkplaceService serverWorkplaceService;

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
                endpoint = options.Endpoint;

                using (var serverUserService = new ServerUserService(endpoint))
                using (var channelManager = serverUserService.CreateChannelManager())
                using (var channel = channelManager.CreateChannel())
                {
                    sessionId = Guid.Parse(options.SessionId);
                    currentUser = channel.Service.OpenUserSession(sessionId).Result as QueueAdministrator;
                }

                container.RegisterInstance<User>(currentUser);
                container.RegisterInstance<QueueAdministrator>(currentUser);

                RegisterServices();

                Application.Run(new AdministratorForm());
            }
            else
            {
                while (true)
                {
                    var loginForm = new LoginForm(UserRole.Administrator);
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        configuration.Save();

                        endpoint = loginSettings.Endpoint;
                        currentUser = loginForm.CurrentUser as QueueAdministrator;
                        sessionId = currentUser.SessionId;

                        container.RegisterInstance<User>(currentUser);
                        container.RegisterInstance<QueueAdministrator>(currentUser);

                        RegisterServices();

                        loginForm.Dispose();

                        using (var f = new AdministratorForm())
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

        private static void RegisterServices()
        {
            serverService = new ServerService(endpoint);
            container.RegisterInstance(serverService);
            container.RegisterType<DuplexChannelManager<IServerTcpService>>
                (new InjectionFactory(c => serverService.CreateChannelManager(sessionId)));

            serverUserService = new ServerUserService(endpoint);
            container.RegisterInstance(serverUserService);
            container.RegisterType<ChannelManager<IServerUserTcpService>>
                (new InjectionFactory(c => serverUserService.CreateChannelManager(sessionId)));

            serverTemplateService = new ServerTemplateService(endpoint);
            container.RegisterInstance(serverTemplateService);
            container.RegisterType<ChannelManager<IServerTemplateTcpService>>
                (new InjectionFactory(c => serverTemplateService.CreateChannelManager(sessionId)));

            serverWorkplaceService = new ServerWorkplaceService(endpoint);
            container.RegisterInstance(serverWorkplaceService);
            container.RegisterType<ChannelManager<IServerWorkplaceTcpService>>
                (new InjectionFactory(c => serverWorkplaceService.CreateChannelManager(sessionId)));
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