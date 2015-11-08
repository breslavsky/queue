using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Core;
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

namespace Queue.Terminal.ViewModels
{
    public class MainWindowViewModel : RichViewModel, IDisposable
    {
        private const double ActivityWaitTime = 120;
        private bool disposed = false;

        private LoginPage loginPage;
        private Navigator navigator;

        private UserService userService;
        private ServerService serverService;
        private TemplateService templateService;
        private TemplateManager templateManager;
        private CommonTemplateManager commonTemplateManager;

        private DispatcherTimer resetTimer;

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

            CreateResetTimer();

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;
            Application.Current.MainWindow.PreviewMouseUp += MainWindow_PreviewMouseUp;

            Window.MakeFullScreen();
        }

        private void CreateResetTimer()
        {
            resetTimer = new DispatcherTimer(DispatcherPriority.Background);
            resetTimer.Tick += ResetTimerTick;
            resetTimer.Interval = TimeSpan.FromSeconds(ActivityWaitTime);
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
            UnityContainer.RegisterInstance(serverService)
                        .RegisterInstance(userService)
                        .RegisterInstance(templateService);
        }

        private void RegisterChannelManagers()
        {
            UnityContainer.RegisterType<ChannelManager<IServerTcpService>>
                            (new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager(loginPage.Model.User.SessionId)))
                        .RegisterType<ChannelManager<ITemplateTcpService>>
                            (new InjectionFactory(c => c.Resolve<TemplateService>().CreateChannelManager()))
                        .RegisterType<ChannelManager<IUserTcpService>>
                            (new InjectionFactory(c => c.Resolve<UserService>().CreateChannelManager()));
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