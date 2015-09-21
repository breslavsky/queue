using Junte.Configuration;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
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
    public partial class MainWindow : RichWindow
    {
        private LoginPage loginPage;

        protected override Panel RootElement { get { return mainGrid; } }

        public MainWindow()
            : base()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>()
                                    .RegisterInstance<IMainWindow>(this);

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
            var container = ServiceLocator.Current.GetInstance<IUnityContainer>();

            container.RegisterInstance<DuplexChannelBuilder<IServerTcpService>>(loginPage.Model.ChannelBuilder);
            container.RegisterInstance<Workplace>(loginPage.Model.Workplace);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && (e.Key == Key.Escape))
            {
                var configuration = ServiceLocator.Current.GetInstance<ConfigurationManager>();
                var loginFormSettings = configuration.GetSection<AppSettings>(AppSettings.SectionKey);
                loginFormSettings.IsRemember = false;
                configuration.Save();

                Application.Current.Shutdown();
            }
        }
    }
}