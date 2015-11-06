using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using MahApps.Metro;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Display.Models;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
using Queue.UI.WPF.Core;
using System;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.Display.ViewModels
{
    public class LoginPageViewModel : DependencyObservableObject, IDisposable
    {
        private bool disposed;
        private AccentColorComboBoxItem selectedAccent;
        private bool isConnected;

        private string endpoint;
        private bool isRemember;
        private RichPage owner;
        private IdentifiedEntityLink[] workplaces;
        private Guid selectedWorkplace;

        private ServerWorkplaceService serverWorkplaceService;
        private ChannelManager<IServerWorkplaceTcpService> channelManager;

        public event EventHandler OnLogined = delegate { };

        private Language selectedLanguage;

        public string Endpoint
        {
            get { return endpoint; }
            set { SetProperty(ref endpoint, value); }
        }

        public bool IsRemember
        {
            get { return isRemember; }
            set { SetProperty(ref isRemember, value); }
        }

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetProperty(ref isConnected, value); }
        }

        public IdentifiedEntityLink[] Workplaces
        {
            get { return workplaces; }
            set { SetProperty(ref workplaces, value); }
        }

        public Guid SelectedWorkplace
        {
            get { return selectedWorkplace; }
            set { SetProperty(ref selectedWorkplace, value); }
        }

        public AccentColorComboBoxItem[] AccentColors { get; set; }

        public AccentColorComboBoxItem SelectedAccent
        {
            get { return selectedAccent; }
            set
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(value.Name);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);

                SetProperty(ref selectedAccent, value);
            }
        }

        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                SetProperty(ref selectedLanguage, value);

                var culture = selectedLanguage.GetCulture();

                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = culture;
                selectedLanguage.SetCurrent();
            }
        }

        public Workplace Workplace { get; private set; }

        public ICommand ConnectCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        [Dependency]
        public AppSettings Settings { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        public LoginPageViewModel(RichPage owner)
            : base()
        {
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
            ConnectCommand = new RelayCommand(Connect);
            LoginCommand = new RelayCommand(Login);
        }

        private void Loaded()
        {
            LoadSettings();

            if (IsRemember || (SelectedWorkplace != Guid.Empty))
            {
                Connect();
            }
        }

        private void LoadSettings()
        {
            Endpoint = Settings.Endpoint;
            SelectedWorkplace = Settings.WorkplaceId;
            SelectedLanguage = Settings.Language;
            IsRemember = Settings.IsRemember;

            if (!String.IsNullOrWhiteSpace(Settings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == Settings.Accent);
            }
        }

        private async void Connect()
        {
            if (serverWorkplaceService != null)
            {
                serverWorkplaceService.Dispose();
            }

<<<<<<< HEAD
            ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new QueuePlanCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
=======
            serverWorkplaceService = new ServerWorkplaceService(Endpoint);
>>>>>>> origin/master

            if (channelManager != null)
            {
                channelManager.Dispose();
            }

            channelManager = serverWorkplaceService.CreateChannelManager();

            IsConnected = false;

            using (var channel = channelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();

                try
                {
                    Workplaces = await TaskPool.AddTask(channel.Service.GetWorkplacesLinks());

                    SelectedWorkplace = Settings.WorkplaceId != Guid.Empty ? Settings.WorkplaceId : Workplaces.First().Id;

                    IsConnected = true;
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(null, exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
                }
            }

            if (isConnected && IsRemember)
            {
                Login();
            }
        }

        private async void Login()
        {
            if (SelectedWorkplace == Guid.Empty)
            {
                return;
            }

            var loading = Window.ShowLoading();

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Workplace = await TaskPool.AddTask(channel.Service.GetWorkplace(SelectedWorkplace));

                    SaveSettings();

                    OnLogined(this, null);
                }
                catch (OperationCanceledException) { }
                catch (CommunicationObjectAbortedException) { }
                catch (ObjectDisposedException) { }
                catch (InvalidOperationException) { }
                catch (FaultException exception)
                {
                    UIHelper.Warning(null, exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(null, exception.Message);
                }
                finally
                {
                    loading.Hide();
                }
            }
        }

        private void SaveSettings()
        {
            Settings.Endpoint = Endpoint;
            Settings.WorkplaceId = SelectedWorkplace;
            Settings.IsRemember = IsRemember;
            Settings.Accent = SelectedAccent == null ? String.Empty : SelectedAccent.Name;
            Settings.Language = SelectedLanguage;

            ConfigurationManager.Save();
        }

        private void Unloaded()
        {
            Dispose();
        }

        #region IDisposable

        ~LoginPageViewModel()
        {
            Dispose(false);
        }

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
                TaskPool.Dispose();

                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            disposed = true;
        }

        #endregion IDisposable
    }
}