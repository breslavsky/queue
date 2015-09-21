using Junte.Configuration;
using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Notification.ViewModels;
using Queue.Notification.Views;
using Queue.Services.Contracts;
using Queue.UI.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace Queue.Notification
{
    public partial class MainWindow : RichWindow
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private LoginPage connectPage;

        protected override Panel RootElement { get { return mainGrid; } }

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

            MakeFullScreen();
        }

        private void RegisterServices()
        {
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();

            container.RegisterInstance<IMainWindow>(this);
            container.RegisterInstance(new ServerService(connectPage.Model.Endpoint, ServerServicesPaths.Server));
            container.RegisterInstance(new ServerTemplateService(connectPage.Model.Endpoint, ServerServicesPaths.Template));
            container.RegisterType<DuplexChannelManager<IServerTcpService>>(new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager()));
            container.RegisterType<ChannelManager<IServerTemplateTcpService>>(new InjectionFactory(c => c.Resolve<ServerTemplateService>().CreateChannelManager()));
            container.RegisterType<TaskPool>();
            container.RegisterInstance<ClientRequestsListener>(new ClientRequestsListener());
            container.RegisterInstance<ITemplateManager>(new TemplateManager(Product.Notification.AppName));
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
    }
}