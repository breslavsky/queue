using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using System.Windows;

namespace Queue.UI.WPF
{
    public class RichApplication : Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            logger.Info("starting application...");

            InitializeContainer();

            logger.Info("application started");
        }

        private void InitializeContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            RegistrateTypes(container);
        }

        protected virtual void RegistrateTypes(IUnityContainer container)
        {
        }

        protected override void OnExit(ExitEventArgs e)
        {
            logger.Info("application stopped");

            base.OnExit(e);
        }
    }
}