using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using System.Windows;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Terminal
{
    public partial class App : Application
    {
        public App()
            : base()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            var container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            container.RegisterInstance(new ConfigurationManager(Product.Terminal.AppName, SpecialFolder.ApplicationData));
        }
    }
}