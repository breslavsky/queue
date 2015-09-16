﻿using Junte.Configuration;
using Junte.Parallel;
using Junte.WCF;
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Notification.ViewModels;
using Queue.Notification.Views;
using Queue.Services.Contracts;
using Queue.UI.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace Queue.Notification
{
    public partial class MainWindow : MetroWindow
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed = false;

        private LoginPage connectPage;
        private TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;

        public MainWindow()
            : base()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            connectPage = CreateConnectPage();
            content.NavigationService.Navigate(connectPage);
        }

        private LoginPage CreateConnectPage()
        {
            var result = new LoginPage();

            result.Model.OnConnected += OnConnected;
            return result;
        }

        private void OnConnected(object sender, EventArgs e)
        {
            RegisterServices();

            content.NavigationService.Navigate(new HomePage());

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;

            var screen = WinForms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Left = screen.WorkingArea.Left;
                Top = screen.WorkingArea.Top;
            }

            this.FullScreenWindow();
        }

        private void RegisterServices()
        {
            var container = ServiceLocator.Current.GetInstance<UnityContainer>();

            taskPool = new TaskPool();
            container.RegisterInstance(connectPage.Model.ChannelBuilder);
            container.RegisterInstance(taskPool);

            var channelBuilder = container.Resolve<DuplexChannelBuilder<IServerTcpService>>();
            channelManager = new ChannelManager<IServerTcpService>(channelBuilder);
            container.RegisterInstance(channelManager);
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Escape)
            {
                var configuration = ServiceLocator.Current.GetInstance<ConfigurationManager>();
                var loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
                loginFormSettings.IsRemember = false;
                configuration.Save();

                Application.Current.Shutdown();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (taskPool != null)
            {
                taskPool.Cancel();
            }

            Dispose();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (taskPool != null)
                    {
                        taskPool.Dispose();
                    }
                    if (channelManager != null)
                    {
                        channelManager.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~MainWindow()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}