using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.UI.WinForms;
using System;
using System.Globalization;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Administrator
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegisterContainer();

            while (true)
            {
                using (LoginForm loginForm = new LoginForm(UserRole.Administrator))
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        AdministratorForm mainForm = new AdministratorForm(loginForm.ChannelBuilder, (QueueAdministrator)loginForm.User);
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
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IConfigurationManager>(new ConfigurationManager());
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        private static void ResetSettings()
        {
            LoginForm.ResetSettings();
            LoginSettingsControl.ResetSettings();
        }
    }
}