using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common.Settings;
using Queue.Notification.Utils;
using Queue.Notification.Views;
using Queue.Services.Contracts.Hub;
using Queue.Services.Contracts.Server;
using Queue.UI.WPF;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WPFLocalizeExtension.Engine;
using WinForms = System.Windows.Forms;

namespace Queue.Notification.ViewModels
{
    public class MainWindowViewModel : RichViewModel, IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private LoginPage connectPage;
        private OperatorDisplayTextUpdater operatorDisplayTextUpdater;

        private ServerService serverService;
        private TemplateService templateService;
        private DisplayService displayService;
        private QueuePlanService queuePlanService;

        private string title;
        private bool disposed;

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
        public HubSettings HubSettings { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        public MainWindowViewModel() :
            base()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            LocalizeDictionary.Instance.PropertyChanged += Instance_PropertyChanged;

            UpdateTitle();
        }

        private void Loaded()
        {
            connectPage = CreateConnectPage();
            Window.Navigate(connectPage);
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

            operatorDisplayTextUpdater = new OperatorDisplayTextUpdater();
            Window.Navigate(new MainPage());

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;

            var screen = WinForms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Application.Current.MainWindow.Left = screen.WorkingArea.Left;
                Application.Current.MainWindow.Top = screen.WorkingArea.Top;
            }

            Window.MakeFullScreen();
        }

        private void RegisterServices()
        {
            serverService = new ServerService(AppSettings.Endpoint);
            templateService = new TemplateService(AppSettings.Endpoint);
            displayService = new DisplayService(HubSettings.Endpoint);
            queuePlanService = new QueuePlanService(AppSettings.Endpoint);

            UnityContainer.RegisterInstance(serverService);
            UnityContainer.RegisterInstance(templateService);
            UnityContainer.RegisterInstance(displayService);
            UnityContainer.RegisterInstance(queuePlanService);

            UnityContainer.RegisterType<ChannelManager<IServerTcpService>>
                (new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager()));
            UnityContainer.RegisterType<DuplexChannelManager<IQueuePlanTcpService>>
                (new InjectionFactory(c => c.Resolve<QueuePlanService>().CreateChannelManager()));
            UnityContainer.RegisterType<ChannelManager<ITemplateTcpService>>
                (new InjectionFactory(c => c.Resolve<TemplateService>().CreateChannelManager()));
            UnityContainer.RegisterType<ChannelManager<IDisplayTcpService>>
                (new InjectionFactory(c => c.Resolve<DisplayService>().CreateChannelManager()));

            UnityContainer.RegisterInstance<ClientRequestsListener>(new ClientRequestsListener());
            UnityContainer.RegisterInstance<ITemplateManager>(new TemplateManager("notification", AppSettings.Theme));
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
            if (operatorDisplayTextUpdater != null)
            {
                operatorDisplayTextUpdater.Dispose();
            }
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
                    displayService.Dispose();
                    templateService.Dispose();
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