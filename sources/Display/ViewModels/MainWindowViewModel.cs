using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Display.Models;
using Queue.Display.Views;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WPFLocalizeExtension.Engine;

namespace Queue.Display.ViewModels
{
    public class MainWindowViewModel : RichViewModel
    {
        private bool disposed;

        private LoginPage loginPage;

        private WorkplaceService workplaceService;
        private ServerService serverService;
        private QueuePlanService queuePlanService;

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public AppSettings AppSettings { get; set; }

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        public MainWindowViewModel()
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

        private LoginPage CreateLoginPage()
        {
            var result = new LoginPage();
            result.Model.OnLogined += OnLogined;

            return result;
        }

        private void OnLogined(object sender, EventArgs e)
        {
            RegisterTypes();

            Window.Navigate(new HomePage()
            {
                DataContext = new HomePageViewModel()
            });

            Application.Current.MainWindow.KeyDown += OnKeyDown;

            var screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Application.Current.MainWindow.Left = screen.WorkingArea.Left;
                Application.Current.MainWindow.Top = screen.WorkingArea.Top;
            }

            Window.MakeFullScreen();
        }

        private void RegisterTypes()
        {
            serverService = new ServerService(AppSettings.Endpoint);
            workplaceService = new WorkplaceService(AppSettings.Endpoint);
            queuePlanService = new QueuePlanService(AppSettings.Endpoint);

            UnityContainer.RegisterInstance(serverService);
            UnityContainer.RegisterInstance(workplaceService);
            UnityContainer.RegisterInstance(queuePlanService);

            UnityContainer.RegisterType<ChannelManager<IServerTcpService>>
                (new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager()));
            UnityContainer.RegisterType<ChannelManager<IWorkplaceTcpService>>
                (new InjectionFactory(c => c.Resolve<WorkplaceService>().CreateChannelManager()));
            UnityContainer.RegisterType<DuplexChannelManager<IQueuePlanTcpService>>
                (new InjectionFactory(c => c.Resolve<QueuePlanService>().CreateChannelManager()));

            UnityContainer.RegisterInstance<Workplace>(loginPage.Model.Workplace);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && (e.Key == Key.Escape))
            {
                AppSettings.IsRemember = false;
                ConfigurationManager.Save();

                Application.Current.Shutdown();
            }
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
            var appName = (string)LocalizeDictionary.Instance.GetLocalizedObject(typeof(MainWindowViewModel).Assembly.FullName,
                                                                 "Strings",
                                                                 "AppName",
                                                                 LocalizeDictionary.Instance.Culture);

            var version = Assembly.GetEntryAssembly().GetName().Version;

            Title = String.Format("{0} ({1})", appName, version);
        }

        private void Unloaded()
        {
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
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                try
                {
                    serverService.Dispose();
                    workplaceService.Dispose();
                    queuePlanService.Dispose();
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