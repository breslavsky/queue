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
        private UnityContainer container;
        private ConfigurationManager configuration;

        public App()
            : base()
        {
            Startup += App_Startup;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(Product.Terminal.AppName, SpecialFolder.ApplicationData);
            container.RegisterInstance(configuration);
        }
    }
}