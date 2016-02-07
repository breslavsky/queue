using Junte.Configuration;
using Junte.UI.WPF;
using MahApps.Metro;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Notification.Settings;
using Queue.Services.Contracts.Server;
using Queue.UI.WPF;
using Queue.UI.WPF.Types;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;
using WinForms = System.Windows.Forms;

namespace Queue.Notification.ViewModels
{
    public class LoginPageViewModel : RichViewModel
    {
        private bool isRemember;
        private bool isFullScreen;

        private string endpoint;
        private AccentColorComboBoxItem selectedAccent;
        private ScreenNumberItem selectedScreenNumber;

        public event EventHandler OnConnected = delegate { };

        private Language selectedLanguage;

        public bool IsRemember
        {
            get { return isRemember; }
            set { SetProperty(ref isRemember, value); }
        }

        public bool IsFullScreen
        {
            get { return isFullScreen; }
            set { SetProperty(ref isFullScreen, value); }
        }

        public string Endpoint
        {
            get { return endpoint; }
            set { SetProperty(ref endpoint, value); }
        }

        public ScreenNumberItem[] ScreensNumbers { get; set; }

        public ScreenNumberItem SelectedScreenNumber
        {
            get { return selectedScreenNumber; }
            set { SetProperty(ref selectedScreenNumber, value); }
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

                var culture = selectedLanguage.GetCulture();

                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = culture;
                selectedLanguage.SetCurrent();
            }
        }

        [Dependency]
        public AppSettings AppSettings { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        public LoginPageViewModel()
            : base()
        {
            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();
            ScreensNumbers = WinForms.Screen.AllScreens.Select((s, pos) => new ScreenNumberItem((byte)pos)).ToArray();

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
            IsRemember = AppSettings.IsRemember;
            IsFullScreen = AppSettings.IsFullScreen;
            SelectedLanguage = AppSettings.Language;
            Endpoint = AppSettings.Endpoint;
            SelectedScreenNumber = ScreensNumbers.FirstOrDefault(d => d.Number == AppSettings.ScreenNumber);

            if (!String.IsNullOrWhiteSpace(AppSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == AppSettings.Accent);
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
                using (var service = new ServerService(Endpoint))
                using (var channelManager = service.CreateChannelManager())
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
            AppSettings.Endpoint = Endpoint;
            AppSettings.IsRemember = IsRemember;
            AppSettings.IsFullScreen = IsFullScreen;
            AppSettings.Accent = SelectedAccent == null ? String.Empty : SelectedAccent.Name;
            AppSettings.Language = SelectedLanguage;
            AppSettings.ScreenNumber = SelectedScreenNumber == null ? (byte)0 : SelectedScreenNumber.Number;

            ConfigurationManager.Save();
        }
    }
}