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
using Queue.UI.WPF.Types;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace Queue.Notification
{
    public partial class MainWindow : MetroWindow, IMainWindow
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed = false;

        private LoginPage connectPage;

        private LoadingControl loading;
        private UserControl activeMessageBox;

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

            content.NavigationService.Navigate(new MainPage());

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

            container.RegisterInstance<IMainWindow>(this);
            container.RegisterInstance(new ServerService(connectPage.Model.Endpoint, ServerServicesPaths.Server));
            container.RegisterInstance(new ServerTemplateService(connectPage.Model.Endpoint, ServerServicesPaths.Template));
            container.RegisterType<DuplexChannelManager<IServerTcpService>>(new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager()));
            container.RegisterType<ChannelManager<IServerTemplateTcpService>>(new InjectionFactory(c => c.Resolve<ServerTemplateService>().CreateChannelManager()));
            container.RegisterType<TaskPool>();
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
            Dispose();
        }

        #region IMainWindow

        public LoadingControl ShowLoading()
        {
            if (loading != null)
            {
                return loading;
            }

            Invoke(() =>
            {
                var control = new LoadingControl();
                AttachControl(control);
                loading = control;
            });

            return loading;
        }

        public void HideLoading()
        {
            if (loading == null)
            {
                return;
            }

            DetachControl(loading);

            loading = null;
        }

        private void HideActiveMessageBox()
        {
            if (activeMessageBox != null)
            {
                HideMessageBox(activeMessageBox);
            }
        }

        public NoticeControl Notice(object message, Action callback = null)
        {
            return ShowMessageBox(() => new NoticeControl(message.ToString(), callback));
        }

        public WarningControl Warning(object message, Action callback = null)
        {
            return ShowMessageBox(() => new WarningControl(message.ToString(), callback));
        }

        public T ShowMessageBox<T>(Func<T> ctor) where T : UserControl
        {
            HideActiveMessageBox();

            T box = null;
            Invoke(() =>
            {
                box = ctor();
                AttachControl(box);
            });

            activeMessageBox = box;

            return box;
        }

        public void HideMessageBox(UserControl control)
        {
            DetachControl(control);
            activeMessageBox = null;
        }

        public void AttachControl(UserControl control)
        {
            Invoke(() => mainGrid.Children.Add(control));
        }

        public void DetachControl(UserControl control)
        {
            Invoke(() => mainGrid.Children.Remove(control));
        }

        public void Invoke(Action action)
        {
            if (Dispatcher.CheckAccess())
            {
                action();
            }
            else
            {
                Dispatcher.Invoke(action);
            }
        }

        #endregion IMainWindow

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