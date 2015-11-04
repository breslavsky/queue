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
using QueueAdministrator = Queue.Services.DTO.Administrator;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Simulator
{
    internal static class Program
    {
        private static UnityContainer container;
        private static ConfigurationManager configuration;
        private static HubSettings hubSettings;
        private static LoginSettings loginSettings;
        private static LoginFormSettings loginFormSettings;

        private static string endpoint;
        private static Guid sessionId;
        private static QueueAdministrator currentUser;

        private static ServerService serverService;
        private static ServerUserService serverUserService;

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(Product.Simulator.AppName, SpecialFolder.ApplicationData);
            container.RegisterInstance(configuration);

            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            container.RegisterInstance(loginSettings);

            loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
            container.RegisterInstance(loginFormSettings);

            hubSettings = configuration.GetSection<HubSettings>(HubSettings.SectionKey);
            container.RegisterInstance(hubSettings);

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

                    using (var f = new SimulatorForm())
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

        private static void RegisterServices()
        {
            serverUserService = new ServerUserService(endpoint);
            container.RegisterInstance(serverUserService);
            container.RegisterType<ChannelManager<IServerUserTcpService>>
                (new InjectionFactory(c => serverUserService.CreateChannelManager(sessionId)));

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