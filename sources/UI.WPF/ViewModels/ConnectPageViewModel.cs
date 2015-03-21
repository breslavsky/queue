using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using MahApps.Metro;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.UI.WPF.Models;
using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.UI.WPF.ViewModels
{
    public class ConnectPageViewModel : ObservableObject, IDisposable
    {
        private RichPage owner;
        private bool isRemember;
        private string endpoint;
        private AccentColorComboBoxItem selectedAccent;

        private TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;

        public event EventHandler OnConnected;

        private IConfigurationManager configuration;
        private LoginSettings loginSettings;
        private LoginFormSettings loginFormSettings;
        private Language selectedLanguage;

        public bool IsRemember
        {
            get { return isRemember; }
            set { SetProperty(ref isRemember, value); }
        }

        public string Endpoint
        {
            get { return endpoint; }
            set { SetProperty(ref endpoint, value); }
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

        public ICommand ConnectCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

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

        public ConnectPageViewModel(RichPage owner)
        {
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();
            taskPool = new TaskPool();

            ConnectCommand = new RelayCommand(Connect);
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            LoadSettings();

            if (IsRemember)
            {
                Connect();
            }
        }

        private void LoadSettings()
        {
            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            Endpoint = loginSettings.Endpoint;

            loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
            IsRemember = loginFormSettings.IsRemember;
            SelectedLanguage = loginFormSettings.Language;

            if (!string.IsNullOrWhiteSpace(loginFormSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == loginFormSettings.Accent);
            }
        }

        private void Connect()
        {
            ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
            channelManager = new ChannelManager<IServerTcpService>(ChannelBuilder);

            SaveSettings();

            if (OnConnected != null)
            {
                OnConnected(this, null);
            }
        }

        private void SaveSettings()
        {
            loginSettings.Endpoint = Endpoint;
            loginFormSettings.IsRemember = IsRemember;
            loginFormSettings.Accent = SelectedAccent == null ? string.Empty : SelectedAccent.Name;
            loginFormSettings.Language = SelectedLanguage;

            configuration.Save();
        }

        private void Unloaded()
        {
            Dispose();
        }

        public void Dispose()
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
    }
}