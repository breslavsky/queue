﻿using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.Core.Settings;
using Queue.Terminal.Views;
using Queue.UI.WPF;
using System;
using System.ComponentModel;
using System.Reflection;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using WPFLocalizeExtension.Engine;
using WinForms = System.Windows.Forms;

namespace Queue.Terminal.ViewModels
{
    public class MainWindowViewModel : RichViewModel, IDisposable
    {
        private const double ActivityWaitTime = 120;
        private const double ServiceBreakPingTime = 30;
        private bool disposed = false;

        private LoginPage loginPage;
        private Navigator navigator;

        private UserService userService;
        private ServerService serverService;
        private TemplateService templateService;
        private LifeSituationService lifeSituationService;
        private TemplateManager templateManager;
        private CommonTemplateManager commonTemplateManager;
        private DispatcherTimer resetTimer;
        private DispatcherTimer serviceBreakTimer;
        private bool isServiceBreak;

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        [Dependency]
        public AppSettings AppSettings { get; set; }

        public MainWindowViewModel()
            : base()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            LocalizeDictionary.Instance.PropertyChanged += Instance_PropertyChanged;

            UpdateTitle();
        }

        private void Loaded()
        {
            loginPage = CreateLoginPage();
            Window.Navigate(loginPage);
        }

        private void Unloaded()
        {
            Dispose();
        }

        private LoginPage CreateLoginPage()
        {
            var result = new LoginPage(UserRole.Administrator);
            result.Model.OnLogined += OnLogined;

            return result;
        }

        private async void OnLogined(object sender, EventArgs e)
        {
            await RegisterTypes();

            Window.Navigate(new TerminalWindow());

            CreateTimers();

            if (AppSettings.ScreenNumber < WinForms.Screen.AllScreens.Length)
            {
                var screen = WinForms.Screen.AllScreens[AppSettings.ScreenNumber];
                Application.Current.MainWindow.Left = screen.WorkingArea.Left;
                Application.Current.MainWindow.Top = screen.WorkingArea.Top;
            }

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.PreviewMouseUp += MainWindow_PreviewMouseUp;

            Window.MakeFullScreen();
        }

        private void CreateTimers()
        {
            resetTimer = new DispatcherTimer(DispatcherPriority.Background);
            resetTimer.Tick += ResetTimerTick;
            resetTimer.Interval = TimeSpan.FromSeconds(ActivityWaitTime);

            if (AppSettings.ServiceBreaks.Count > 0)
            {
                serviceBreakTimer = new DispatcherTimer(DispatcherPriority.Background);
                serviceBreakTimer.Tick += ServiceBreakTimerTick;
                serviceBreakTimer.Interval = TimeSpan.FromSeconds(ServiceBreakPingTime);
                serviceBreakTimer.Start();
            }
        }

        private void ServiceBreakTimerTick(object sender, EventArgs e)
        {
            var time = DateTime.Now.TimeOfDay;
            var currentServiceBreak = GetServiceBreakForNow();

            if (currentServiceBreak != null)
            {
                if (!isServiceBreak)
                {
                    navigator.Reset();
                    resetTimer.Stop();
                    var msg = String.IsNullOrWhiteSpace(currentServiceBreak.Message) ?
                                        "В данный момент услуга не оказывается" :
                                        currentServiceBreak.Message;

                    Window.Warning(msg, null, false);

                    isServiceBreak = true;
                }
            }
            else
            {
                if (isServiceBreak)
                {
                    Window.HideActiveMessageBox();
                    isServiceBreak = false;
                }
            }
        }

        private ServiceBreak GetServiceBreakForNow()
        {
            var time = DateTime.Now.TimeOfDay;
            foreach (ServiceBreak serviceBreak in AppSettings.ServiceBreaks)
            {
                if (time >= serviceBreak.From && time <= serviceBreak.To)
                {
                    return serviceBreak;
                }
            }

            return null;
        }

        private void MainWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            resetTimer.Stop();
            resetTimer.Start();
        }

        private async Task RegisterTypes()
        {
            RegisterServices();
            RegisterChannelManagers();
            RegisterTemplateManagers();

            UnityContainer.RegisterInstance(new ClientRequestModel((Administrator)loginPage.Model.User));

            navigator = UnityContainer.Resolve<Navigator>();
            UnityContainer.RegisterInstance(navigator);
            await RegisterConfigs();
        }

        private void RegisterServices()
        {
            serverService = new ServerService(AppSettings.Endpoint);
            userService = new UserService(AppSettings.Endpoint);
            templateService = new TemplateService(AppSettings.Endpoint);
            lifeSituationService = new LifeSituationService(AppSettings.Endpoint);

            UnityContainer.RegisterInstance(serverService)
                        .RegisterInstance(userService)
                        .RegisterInstance(templateService)
                        .RegisterInstance(lifeSituationService);
        }

        private void RegisterChannelManagers()
        {
            UnityContainer.RegisterType<ChannelManager<IServerTcpService>>
                            (new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager(loginPage.Model.User.SessionId)))
                        .RegisterType<ChannelManager<ITemplateTcpService>>
                            (new InjectionFactory(c => c.Resolve<TemplateService>().CreateChannelManager()))
                        .RegisterType<ChannelManager<IUserTcpService>>
                            (new InjectionFactory(c => c.Resolve<UserService>().CreateChannelManager()))
                        .RegisterType<ChannelManager<ILifeSituationTcpService>>
                            (new InjectionFactory(c => c.Resolve<LifeSituationService>().CreateChannelManager()));
        }

        private void RegisterTemplateManagers()
        {
            templateManager = new TemplateManager(Templates.Apps.Terminal, AppSettings.Theme);
            commonTemplateManager = new CommonTemplateManager(AppSettings.Theme);
            UnityContainer.RegisterInstance<ITemplateManager>(templateManager)
                            .RegisterInstance<ICommonTemplateManager>(commonTemplateManager);
        }

        private async Task RegisterConfigs()
        {
            using (var manager = UnityContainer.Resolve<ChannelManager<IServerTcpService>>())
            using (var channel = manager.CreateChannel())
            {
                try
                {
                    UnityContainer.RegisterInstance(await channel.Service.GetTerminalConfig());
                    UnityContainer.RegisterInstance(await channel.Service.GetDefaultConfig());
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

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Escape)
            {
                AppSettings.IsRemember = false;
                ConfigurationManager.Save();

                Application.Current.Shutdown();
            }
        }

        private void ResetTimerTick(object sender, EventArgs e)
        {
            navigator.Reset();
            resetTimer.Stop();
        }

        private void Instance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Culture")
            {
                UpdateTitle();
            }
        }

        private void UpdateTitle()
        {
            var appName = (string)LocalizeDictionary.Instance.GetLocalizedObject(GetType().Assembly.FullName,
                                                                 "Strings",
                                                                 "AppName",
                                                                 LocalizeDictionary.Instance.Culture);

            var version = Assembly.GetEntryAssembly().GetName().Version;

            Title = String.Format("{0} ({1})", appName, version);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    serverService.Dispose();
                    templateService.Dispose();
                    userService.Dispose();
                    templateManager.Dispose();
                    lifeSituationService.Dispose();
                    commonTemplateManager.Dispose();

                    resetTimer.Stop();
                    resetTimer = null;
                }
                catch { }
            }

            disposed = true;
        }

        ~MainWindowViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}