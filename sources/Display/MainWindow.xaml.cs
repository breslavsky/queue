using Junte.Configuration;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Display.Models;
using Queue.Display.ViewModels;
using Queue.Display.Views;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Queue.Display
{
    public partial class MainWindow : RichWindow, IMainWindow
    {
        private LoginPage loginPage;

        protected override Panel RootElement { get { return mainGrid; } }

        [Dependency]
        public IUnityContainer UnityContainer { get; set; }

        [Dependency]
        public AppSettings Settings { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        public MainWindow()
            : base()
        {
            InitializeComponent();

            UnityContainer.RegisterInstance<IMainWindow>(this);

            DataContext = new MainWindowViewModel();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            loginPage = CreateLoginPage();
            content.NavigationService.Navigate(loginPage);
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

            content.NavigationService.Navigate(new HomePage()
            {
                DataContext = new HomePageViewModel()
            });

            Application.Current.MainWindow.KeyDown += OnKeyDown;

            var screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Left = screen.WorkingArea.Left;
                Top = screen.WorkingArea.Top;
            }

            this.MakeFullScreen();
        }

        private void RegisterTypes()
        {
            UnityContainer.RegisterInstance(new ServerService(Settings.Endpoint));
            UnityContainer.RegisterType<DuplexChannelManager<IServerTcpService>>
                (new InjectionFactory(c => c.Resolve<ServerService>().CreateChannelManager()));

            UnityContainer.RegisterInstance(new ServerWorkplaceService(Settings.Endpoint));
            UnityContainer.RegisterType<ChannelManager<IServerWorkplaceTcpService>>
                (new InjectionFactory(c => c.Resolve<ServerWorkplaceService>().CreateChannelManager()));

            UnityContainer.RegisterInstance<Workplace>(loginPage.Model.Workplace);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && (e.Key == Key.Escape))
            {
                Settings.IsRemember = false;
                ConfigurationManager.Save();

                Application.Current.Shutdown();
            }
        }

        public void Navigate(Page page)
        {
            content.NavigationService.Navigate(page);
        }
    }
}