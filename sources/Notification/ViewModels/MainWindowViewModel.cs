using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common.Settings;
using Queue.Notification.Utils;
using Queue.Notification.Views;
using Queue.Services.Contracts;
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
    public class MainWindowViewModel : ObservableObject
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private LoginPage connectPage;
        private OperatorDisplayTextUpdater operatorDisplayTextUpdater;

        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IMainWindow window { get; set; }

        public MainWindowViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            LocalizeDictionary.Instance.PropertyChanged += Instance_PropertyChanged;

            UpdateTitle();
        }

        private void Loaded()
        {
            connectPage = CreateConnectPage();
            window.Navigate(connectPage);
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
            window.Navigate(new MainPage());

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;

            var screen = WinForms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Application.Current.MainWindow.Left = screen.WorkingArea.Left;
                Application.Current.MainWindow.Top = screen.WorkingArea.Top;
            }

            window.MakeFullScreen();
        }

        private void RegisterServices()
        {
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();

            container.RegisterInstance(new ServerService(connectPage.Model.Endpoint));
            container.RegisterInstance(new TemplateService(connectPage.Model.Endpoint));
            container.RegisterInstance(new DisplayService(ServiceLocator.Current.GetInstance<HubSettings>().Endpoint));

            container.RegisterType<DuplexChannelManager<IServerTcpService>>
                (new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager()));
            container.RegisterType<ChannelManager<ITemplateTcpService>>
                (new InjectionFactory(c => c.Resolve<TemplateService>().CreateChannelManager()));
            container.RegisterType<ChannelManager<IDisplayTcpService>>
                (new InjectionFactory(c => c.Resolve<IDisplayTcpService>().CreateChannelManager()));

            container.RegisterType<TaskPool>();
            container.RegisterInstance<ClientRequestsListener>(new ClientRequestsListener());
            container.RegisterInstance<ITemplateManager>(new TemplateManager("notification", ServiceLocator.Current.GetInstance<AppSettings>().Theme));
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Escape)
            {
                var configuration = ServiceLocator.Current.GetInstance<ConfigurationManager>();
                var loginFormSettings = configuration.GetSection<AppSettings>(AppSettings.SectionKey);
                loginFormSettings.IsRemember = false;
                configuration.Save();

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
    }
}