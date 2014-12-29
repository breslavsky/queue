using Junte.WCF.Common;
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Display.Models;

using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
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

        public MainWindow()
            : base()
        {
            InitializeComponent();

            Title = string.Format("Информационное табло ({0})", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Properties.Settings settings = Properties.Settings.Default;

            if (Keyboard.IsKeyDown(Key.LeftShift) && (e.Key == Key.Escape))
            {
                settings.IsRemember = false;
                settings.Save();

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
            Properties.Settings settings = Properties.Settings.Default;

            DisplayLoginPage result = new DisplayLoginPage();
            result.Model.ApplySettings(new LoginSettings()
            {
                Endpoint = settings.Endpoint,
                WorkplaceId = settings.WorkplaceId,
                IsRemember = settings.IsRemember,
                Accent = settings.Accent
            });

            result.Model.OnLogined += OnLogined;

            return result;
        }

        private void OnLogined(object sender, EventArgs e)
        {
            SaveSettings();

            InitializeContainer();

            content.NavigationService.Navigate(new HomePage()
                {
                    DataContext = new HomePageVM()
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

        private void SaveSettings()
        {
            Properties.Settings settings = Properties.Settings.Default;

            settings.Endpoint = loginPage.Model.Endpoint;
            settings.IsRemember = loginPage.Model.IsRemember;
            settings.WorkplaceId = loginPage.Model.Workplace.Id;

            if (loginPage.Model.SelectedAccent != null)
            {
                settings.Accent = loginPage.Model.SelectedAccent.Name;
            }

            settings.Save();
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
            container.RegisterInstance<IUnityContainer>(container);
            container.RegisterInstance<Workplace>(loginPage.Model.Workplace);
        }
    }
}