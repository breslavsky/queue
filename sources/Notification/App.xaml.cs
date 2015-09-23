﻿using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using System.Windows;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Notification
{
    public partial class App : Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        public App()
        {
            Startup += App_Startup;
            Exit += App_Exit;
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            logger.Info("starting application...");

            InitializeContainer();

            logger.Info("application started");
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            logger.Info("application stopped");
        }

        private void InitializeContainer()
        {
            var container = new UnityContainer();
            container.RegisterInstance<IUnityContainer>(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            container.RegisterInstance(new ConfigurationManager(Product.Notification.AppName, SpecialFolder.ApplicationData));
            container.RegisterType<AppSettings>(new InjectionFactory(c => c.Resolve<ConfigurationManager>()
                                                                .GetSection<AppSettings>(AppSettings.SectionKey)));
            container.RegisterType<HubSettings>(new InjectionFactory(c => c.Resolve<ConfigurationManager>()
                                                                .GetSection<HubSettings>(HubSettings.SectionKey)));
        }
    }
}