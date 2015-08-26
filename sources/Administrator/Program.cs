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
    public class Options
    {
        [Option]
        public bool AutoLogin { get; set; }

        [Option]
        public string Host { get; set; }

        [Option]
        public int Port { get; set; }

        [Option]
        public string SessionId { get; set; }
    }

    internal static class Program
    {
        private const string AppName = "Queue.Administrator";
        private static Options options;
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
                string host = options.Host;
                int port = options.Port;
                Guid sessionId = Guid.Parse(options.SessionId);

                //var serviceManager = new ServerServiceManager(Schemes.NET_TCP, host, port);
                //var channelManager = serviceManager.Server.CreateChannelManager();
                //using (var channel = channelManager.CreateChannel())
                //{
                //    var currentUser = channel.Service.OpenUserSession(sessionId).Result;
                //    container.RegisterInstance<IServerServiceManager>(serviceManager);
                //    container.RegisterInstance<User>(currentUser);

                //    Application.Run(new AdministratorForm());
                //}
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

                        serverService = new ClientService<IServerTcpService>(loginSettings.Endpoint);
                        container.RegisterInstance<IClientService<IServerTcpService>>(serverService);

                        loginForm.Dispose();

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
        }

        private static void ParseOptions()
        {
            options = new Options();
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