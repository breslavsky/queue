﻿using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using System.Windows;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Notification
{
    public partial class App : Application
    {
        private const string AppName = "Queue.Notification";

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
            IUnityContainer container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
            RegisterTypes(container);
        }

        private void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterInstance<IConfigurationManager>(new ConfigurationManager(AppName, SpecialFolder.ApplicationData));
        }
    }
}