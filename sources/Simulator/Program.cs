using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.UI.WinForms;
using System;
using System.Windows.Forms;
using QueueAdministrator = Queue.Services.DTO.Administrator;

namespace Queue.Simulator
{
    internal static class Program
    {
        private const string AppName = "Queue.Simulator";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            while (true)
            {
                using (LoginForm loginForm = new LoginForm(UserRole.Administrator))
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        MainForm mainForm = new MainForm(loginForm.ChannelBuilder, (QueueAdministrator)loginForm.User);
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
            container.RegisterInstance<IConfigurationManager>(new ConfigurationManager(AppName));
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }

        private static void ResetSettings()
        {
            IConfigurationManager configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();

            configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey).Reset();
            configuration.GetSection<LoginSettings>(LoginSettings.SectionKey).Reset();

            configuration.Save();
        }
    }
}