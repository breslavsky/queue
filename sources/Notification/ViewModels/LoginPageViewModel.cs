using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using MahApps.Metro;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.UI.WPF;
using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.Notification.ViewModels
{
    public class LoginPageViewModel : ObservableObject
    {
        private bool isRemember;

        private string endpoint;
        private AccentColorComboBoxItem selectedAccent;

        public event EventHandler OnConnected = delegate { };

        private ConfigurationManager configuration;
        private LoginSettings loginSettings;
        private AppSettings appSettings;
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
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                var accent = ThemeManager.GetAccent(value.Name);
                ThemeManager.ChangeAppStyle(Application.Current, accent, theme.Item1);

                SetProperty(ref selectedAccent, value);
            }
        }

        public ICommand ConnectCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

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

        public LoginPageViewModel()
        {
            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();

            ConnectCommand = new RelayCommand(Connect);
            LoadedCommand = new RelayCommand(Loaded);
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
            configuration = ServiceLocator.Current.GetInstance<ConfigurationManager>();
            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey);
            Endpoint = loginSettings.Endpoint;

            appSettings = configuration.GetSection<AppSettings>(AppSettings.SectionKey);
            IsRemember = appSettings.IsRemember;
            SelectedLanguage = appSettings.Language;

            if (!String.IsNullOrWhiteSpace(appSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == appSettings.Accent);
            }
        }

        private async void Connect()
        {
            if (!(await ConnectionValid()))
            {
                return;
            }

            SaveSettings();
            OnConnected(this, null);
        }

        private async Task<bool> ConnectionValid()
        {
            try
            {
                using (var channelManager = new DuplexChannelManager<IServerTcpService>(
                                                new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(),
                                                                                            Bindings.NetTcpBinding,
                                                                                            new EndpointAddress(Endpoint))))
                using (var channel = channelManager.CreateChannel())
                {
                    var date = await channel.Service.GetDateTime();
                }

                return true;
            }
            catch (Exception e)
            {
                UIHelper.Error(null, "Не удалось подключиться к серверу", e);
            }

            return false;
        }

        private void SaveSettings()
        {
            loginSettings.Endpoint = Endpoint;
            appSettings.IsRemember = IsRemember;
            appSettings.Accent = SelectedAccent == null ? string.Empty : SelectedAccent.Name;
            appSettings.Language = SelectedLanguage;

            configuration.Save();
        }
    }
}