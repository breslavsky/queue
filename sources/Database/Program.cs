using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using System;
using System.Windows.Forms;

namespace Queue.Database
{
    public static class Program
    {
        private const string AppName = "Queue.Database";

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegisterContainer();

            Application.Run(new MainForm());
        }

        private static void RegisterContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterInstance<IConfigurationManager>(new ConfigurationManager(AppName));
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
        }
    }
}