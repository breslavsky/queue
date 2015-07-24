using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
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

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            RegisterContainer();

            while (true)
            {
                using (LoginForm loginForm = new LoginForm(UserRole.Operator)
                )
                {
                    if (loginForm.ShowDialog() == DialogResult.OK)
                    {
                        OperatorForm mainForm = new OperatorForm(loginForm.ChannelBuilder, (QueueOperator)loginForm.User);
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
            container.RegisterInstance<IConfigurationManager>(new ConfigurationManager(AppName, SpecialFolder.ApplicationData));
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