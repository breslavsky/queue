using Junte.Configuration;
using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using MahApps.Metro;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Display.Models;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.Display.ViewModels
{
    public class LoginPageViewModel : ObservableObject, IDisposable
    {
        private bool disposed;
        private readonly TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;
        private AccentColorComboBoxItem selectedAccent;
        private bool isConnected;

        private string endpoint;
        private bool isRemember;
        private RichPage owner;
        private IdentifiedEntityLink[] workplaces;
        private Guid selectedWorkplace;

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        public event EventHandler OnLogined;

        private ConfigurationManager configuration;
        private DisplayLoginSettings loginSettings;
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
                Tuple<AppTheme, Accent> theme = ThemeManager.DetectAppStyle(Application.Current);
                Accent accent = ThemeManager.GetAccent(value.Name);
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

                CultureInfo culture = selectedLanguage.GetCulture();

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

        public LoginPageViewModel(RichPage owner)
        {
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();

            taskPool = new TaskPool();

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
            configuration = ServiceLocator.Current.GetInstance<ConfigurationManager>();
            loginSettings = configuration.GetSection<DisplayLoginSettings>(DisplayLoginSettings.SectionKey);

            Endpoint = loginSettings.Endpoint;
            SelectedWorkplace = loginSettings.WorkplaceId;
            SelectedLanguage = loginSettings.Language;
            IsRemember = loginSettings.IsRemember;

            if (!string.IsNullOrWhiteSpace(loginSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == loginSettings.Accent);
            }
        }

        private async void Connect()
        {
            if (ChannelBuilder != null)
            {
                ChannelBuilder.Dispose();
            }

            ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));

            if (channelManager != null)
            {
                channelManager.Dispose();
            }

            channelManager = new ChannelManager<IServerTcpService>(ChannelBuilder);

            IsConnected = false;

            using (var channel = channelManager.CreateChannel())
            {
                var loading = owner.ShowLoading();

                try
                {
                    Workplaces = await taskPool.AddTask(channel.Service.GetWorkplacesLinks());

                    SelectedWorkplace = loginSettings.WorkplaceId != Guid.Empty ? loginSettings.WorkplaceId : Workplaces.First().Id;

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

            var loading = owner.ShowLoading();

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Workplace = await taskPool.AddTask(channel.Service.GetWorkplace(SelectedWorkplace));

                    SaveSettings();

                    if (OnLogined != null)
                    {
                        OnLogined(this, null);
                    }
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
            loginSettings.Endpoint = Endpoint;
            loginSettings.WorkplaceId = SelectedWorkplace;
            loginSettings.IsRemember = IsRemember;
            loginSettings.Accent = SelectedAccent == null ? String.Empty : SelectedAccent.Name;
            loginSettings.Language = SelectedLanguage;

            configuration.Save();
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

        #endregion IDisposable
    }
}