﻿using Junte.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using System.Windows;
using SpecialFolder = System.Environment.SpecialFolder;

namespace Queue.Notification
{
    public partial class App : Application
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private UnityContainer container;
        private ConfigurationManager configuration;

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
            container = new UnityContainer();
            container.RegisterInstance(container);
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));

            configuration = new ConfigurationManager(Product.Notification.AppName, SpecialFolder.ApplicationData);
            container.RegisterInstance(configuration);
        }
    }
}