﻿using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.ViewModels;
using Queue.Terminal.Views;
using Queue.UI.WPF;
using Queue.UI.WPF.Models;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Queue.Terminal
{
    public partial class MainWindow : MetroWindow, IDisposable
    {
        private const double ActivityWaitTime = 120;
        private bool disposed = false;

        private LoginPage loginPage;
        private TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;
        private Navigator navigator;

        private DispatcherTimer resetTimer;

        public MainWindow()
            : base()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            loginPage = CreateLoginPage();
            content.NavigationService.Navigate(loginPage);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (taskPool != null)
            {
                taskPool.Cancel();
            }

            Dispose();
        }

        private LoginPage CreateLoginPage()
        {
            LoginPage result = new LoginPage(UserRole.Administrator);
            result.Model.OnLogined += OnLogined;

            return result;
        }

        private async void OnLogined(object sender, EventArgs e)
        {
            await RegisterTypes();

            content.NavigationService.Navigate(new TerminalWindow());

            CreateResetTimer();
            this.FullScreenWindow();
        }

        private void CreateResetTimer()
        {
            resetTimer = new DispatcherTimer(DispatcherPriority.Background);
            resetTimer.Tick += ResetTimerTick;
            resetTimer.Interval = TimeSpan.FromSeconds(ActivityWaitTime);

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.PreviewMouseUp += MainWindow_PreviewMouseUp;
        }

        private void MainWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            resetTimer.Stop();
            resetTimer.Start();
        }

        private async Task RegisterTypes()
        {
            IUnityContainer container = ServiceLocator.Current.GetInstance<IUnityContainer>();

            taskPool = new TaskPool();
            container.RegisterInstance<DuplexChannelBuilder<IServerTcpService>>(loginPage.Model.ChannelBuilder);
            container.RegisterInstance<TaskPool>(taskPool);

            channelManager = new ChannelManager<IServerTcpService>(container.Resolve<DuplexChannelBuilder<IServerTcpService>>());
            container.RegisterInstance<ChannelManager<IServerTcpService>>(channelManager);
            container.RegisterInstance<ClientRequestModel>(new ClientRequestModel()
            {
                CurrentAdministrator = (Administrator)loginPage.Model.User
            });

            navigator = container.Resolve<Navigator>();
            container.RegisterInstance<Navigator>(navigator);
            await LoadConfigs(container);
        }

        private async Task LoadConfigs(IUnityContainer container)
        {
            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                try
                {
                    await channel.Service.OpenUserSession(loginPage.Model.User.SessionId);
                    container.RegisterInstance<TerminalConfig>(await taskPool.AddTask(channel.Service.GetTerminalConfig()));
                    container.RegisterInstance<DefaultConfig>(await taskPool.AddTask(channel.Service.GetDefaultConfig()));
                    container.RegisterInstance<CouponConfig>(await taskPool.AddTask(channel.Service.GetCouponConfig()));
                }
                catch (FaultException exception)
                {
                    UIHelper.Warning(null, exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
            }
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Escape)
            {
                IConfigurationManager configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
                LoginFormSettings loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
                loginFormSettings.IsRemember = false;
                configuration.Save();

                Application.Current.Shutdown();
            }
        }

        private void ResetTimerTick(object sender, EventArgs e)
        {
            navigator.Reset();
            resetTimer.Stop();
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