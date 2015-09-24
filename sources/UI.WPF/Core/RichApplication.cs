using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using NLog.Config;
using NLog.Targets;
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

            ConfigureLogger();
            InitializeContainer();

            logger.Info("application started");
        }

        public void ConfigureLogger()
        {
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget()
            {
                FileName = "${basedir}/logs/log.txt",
                ConcurrentWrites = true,
                KeepFileOpen = false,
                ArchiveFileName = "${basedir}/logs/log.{#}.txt",
                ArchiveAboveSize = 1024 * 1024 * 10,
                ArchiveNumbering = ArchiveNumberingMode.Sequence,
                Layout = "${longdate}|${level:uppercase=true}|${logger}|${message} ${exception:format=tostring}"
            };
            config.AddTarget("file", fileTarget);

            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            LogManager.Configuration = config;
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