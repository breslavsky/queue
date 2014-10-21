using Junte.Parallel.Common;
using Junte.WCF.Common;
using MahApps.Metro.Controls;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using Queue.UI.WPF;
using Queue.UI.WPF.Types;
using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WinForms = System.Windows.Forms;

namespace Queue.Notification
{
    public partial class MainWindow : MetroWindow
    {
        private bool disposed = false;

        private ConnectPage connectPage;
        private TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;

        public MainWindow()
            : base()
        {
            InitializeComponent();

            Title = string.Format("Служба оповещения (ver. {0})", Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            connectPage = CreateConnectPage();
            content.NavigationService.Navigate(connectPage);
        }

        private ConnectPage CreateConnectPage()
        {
            Properties.Settings settings = Properties.Settings.Default;

            ConnectPage result = new ConnectPage();
            result.Model.ApplyUserSettings(new UserLoginSettings()
            {
                Endpoint = settings.Endpoint,
                IsRemember = settings.IsRemember,
                Accent = settings.Accent
            });

            result.Model.OnConnected += OnConnected;
            return result;
        }

        private void OnConnected(object sender, System.EventArgs e)
        {
            SaveSettings();

            InitializeContainer();

            content.NavigationService.Navigate(new HomePage());

            Application.Current.MainWindow.KeyDown += MainWindow_KeyDown;

            WinForms.Screen screen = WinForms.Screen.AllScreens.FirstOrDefault(s => !s.Primary);
            if (screen != null)
            {
                Left = screen.WorkingArea.Left;
                Top = screen.WorkingArea.Top;
            }

            this.MakeFullScreenWindowStyle();
        }

        private void SaveSettings()
        {
            Properties.Settings settings = Properties.Settings.Default;

            settings.Endpoint = connectPage.Model.Endpoint;
            settings.IsRemember = connectPage.Model.IsRemember;
            settings.IsDebug = Keyboard.IsKeyDown(Key.LeftShift);

            if (connectPage.Model.SelectedAccent != null)
            {
                settings.Accent = connectPage.Model.SelectedAccent.Name;
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
            taskPool = new TaskPool();
            container.RegisterInstance<DuplexChannelBuilder<IServerTcpService>>(connectPage.Model.ChannelBuilder);
            container.RegisterInstance<TaskPool>(taskPool);
            container.RegisterInstance<IUnityContainer>(container);

            channelManager = new ChannelManager<IServerTcpService>(container.Resolve<DuplexChannelBuilder<IServerTcpService>>());
            container.RegisterInstance<ChannelManager<IServerTcpService>>(channelManager);
        }

        private void MainWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            Properties.Settings settings = Properties.Settings.Default;
            if (Keyboard.IsKeyDown(Key.LeftShift) && e.Key == Key.Escape)
            {
                settings.IsRemember = false;
                settings.Save();

                Application.Current.Shutdown();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (taskPool != null)
            {
                taskPool.Cancel();
            }

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
            if (!disposed)
            {
                if (disposing)
                {
                    if (taskPool != null)
                    {
                        taskPool.Dispose();
                    }
                    if (channelManager != null)
                    {
                        channelManager.Dispose();
                    }
                }
                disposed = true;
            }
        }

        ~MainWindow()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}