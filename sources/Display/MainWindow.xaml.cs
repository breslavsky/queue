using Junte.WCF.Common;
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Display.ViewModels;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
using Queue.UI.WPF.Models;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using DisplayLoginPage = Queue.Display.Pages.LoginPage;
using WinForms = System.Windows.Forms;

namespace Queue.Display
{
    public partial class MainWindow : MetroWindow
    {
        private DisplayLoginPage loginPage;

        public Version Version { get; set; }

        public MainWindow()
            : base()
        {
            InitializeComponent();

            Version = Assembly.GetExecutingAssembly().GetName().Version;

            DataContext = this;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftShift) && (e.Key == Key.Escape))
            {
                Common.IConfigurationManager configuration = ServiceLocator.Current.GetInstance<Common.IConfigurationManager>();
                LoginFormSettings loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
                loginFormSettings.IsRemember = false;
                configuration.Save();

                Application.Current.Shutdown();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            loginPage = CreateLoginPage();
            content.NavigationService.Navigate(loginPage);
        }

        private DisplayLoginPage CreateLoginPage()
        {
            DisplayLoginPage result = new DisplayLoginPage();
            result.Model.OnLogined += OnLogined;

            return result;
        }

        private void OnLogined(object sender, EventArgs e)
        {
            InitializeContainer();

            content.NavigationService.Navigate(new HomePage()
                {
                    DataContext = new HomePageViewModel()
                });

            Application.Current.MainWindow.KeyDown += OnKeyDown;

            WinForms.Screen screen = System.Windows.Forms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Left = screen.WorkingArea.Left;
                Top = screen.WorkingArea.Top;
            }

            this.FullScreenWindow();
        }

        private void InitializeContainer()
        {
            IUnityContainer container = new UnityContainer();
            ServiceLocator.SetLocatorProvider(() => new UnityServiceLocator(container));
            RegisterTypes(container);
        }

        private void RegisterTypes(IUnityContainer container)
        {
            container.RegisterInstance<DuplexChannelBuilder<IServerTcpService>>(loginPage.Model.ChannelBuilder);
            container.RegisterInstance<Workplace>(loginPage.Model.Workplace);
        }
    }
}