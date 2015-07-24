using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Administrator
{
    internal static class Program
    {
        private const string AppName = "Queue.Administrator";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegisterContainer();

            while (true)
            {
                using (var loginForm = new LoginForm(UserRole.Administrator))
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        var mainForm = new AdministratorForm(loginForm.ChannelBuilder, (QueueAdministrator)loginForm.User);
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

        private static void RegisterContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance<IConfigurationManager>(new ConfigurationManager(AppName, SpecialFolder.ApplicationData));
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