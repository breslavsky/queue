using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using System.Windows;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Display
{
    public partial class App : Application
    {
        private const string AppName = "Queue.Display";

        public App()
            : base()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            IUnityContainer container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            RegisterTypes(container);
        }

        private void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance(container);
            container.RegisterInstance(new ConfigurationManager(AppName, SpecialFolder.ApplicationData));
        }
    }
}