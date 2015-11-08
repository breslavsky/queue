using Junte.Configuration;
using Junte.UI.WPF;
using Junte.WCF;
using MahApps.Metro;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.Terminal.Views;
using Queue.UI.WPF;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.Terminal.ViewModels
{
    public class LoginPageViewModel : RichViewModel, IDisposable
    {
        private bool disposed;

        private LoginPage owner;
        private string endpoint;
        private bool isConnected;
        private string password;
        private bool isRemember;
        private AccentColorComboBoxItem selectedAccent;
        private UserComboBoxItem selectedUser;
        private UserComboBoxItem[] users;
        private UserRole userRole;
        private Language selectedLanguage;

        private ChannelManager<IUserTcpService> channelManager;
        private UserService serverUserService;

        public event EventHandler OnLogined = delegate { };

        #region UIProperties

        public bool IsConnected
        {
            get { return isConnected; }
            set { SetProperty(ref isConnected, value); }
        }

        public string Endpoint
        {
            get { return endpoint; }
            set { SetProperty(ref endpoint, value); }
        }

        public string Password
        {
            get { return password; }
            set { SetProperty(ref password, value); }
        }

        public bool IsRemember
        {
            get { return isRemember; }
            set { SetProperty(ref isRemember, value); }
        }

        public User User { get; set; }

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

        public UserComboBoxItem[] Users
        {
            get { return users; }
            set { SetProperty(ref users, value); }
        }

        public UserComboBoxItem SelectedUser
        {
            get { return selectedUser; }
            set { SetProperty(ref selectedUser, value); }
        }

        public ICommand ConnectCommand { get; set; }

        public ICommand LoginCommand { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        #endregion UIProperties

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        [Dependency]
        public AppSettings Settings { get; set; }

        public LoginPageViewModel(UserRole userRole, LoginPage owner) :
            base()
        {
            this.userRole = userRole;
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();

            ConnectCommand = new RelayCommand(Connect);
            LoginCommand = new RelayCommand(Login);
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            LoadSettings();

            if (IsRemember || (Settings.User != Guid.Empty))
            {
                Connect();
            }
        }

        private void LoadSettings()
        {
            Endpoint = Settings.Endpoint;
            Password = Settings.Password;
            IsRemember = Settings.IsRemember;
            SelectedLanguage = Settings.Language;

            if (!String.IsNullOrWhiteSpace(Settings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == Settings.Accent);
            }

            owner.Adjust();
        }

        public async void Connect()
        {
            serverUserService = new UserService(Endpoint);
            channelManager = serverUserService.CreateChannelManager();

            var result = await Window.ExecuteLongTask(async () =>
                  {
                      using (var channel = channelManager.CreateChannel())
                      {
                          return await channel.Service.GetUserLinks(userRole);
                      }
                  });

            if (result == null)
            {
                return;
            }

            Users = result.Select(u => new UserComboBoxItem(u.Id, u.ToString())).ToArray();

            if (Settings.User != Guid.Empty)
            {
                SelectedUser = Users.SingleOrDefault(u => u.Id == Settings.User);
            }

            if (SelectedUser == null)
            {
                SelectedUser = Users.First();
            }

            IsConnected = true;

            if (IsConnected && IsRemember)
            {
                Login();
            }
        }

        private async void Login()
        {
            if (SelectedUser == null)
            {
                return;
            }

            User = await Window.ExecuteLongTask(async () =>
            {
                using (var channel = channelManager.CreateChannel())
                {
                    return await channel.Service.UserLogin(SelectedUser.Id, Password);
                }
            });

            if (User == null)
            {
                return;
            }

            SaveSettings();
            OnLogined(this, null);
        }

        private void SaveSettings()
        {
            Settings.Endpoint = Endpoint;
            Settings.Password = IsRemember ? Password : String.Empty;
            Settings.User = SelectedUser.Id;
            Settings.IsRemember = IsRemember;
            Settings.Accent = SelectedAccent == null ? String.Empty : SelectedAccent.Name;
            Settings.Language = SelectedLanguage;

            ConfigurationManager.Save();
        }

        private void Unloaded()
        {
            Dispose();
        }

        private void Disconnect()
        {
            try
            {
                if (channelManager != null)
                {
                    channelManager.Dispose();
                    channelManager = null;
                }

                if (serverUserService != null)
                {
                    serverUserService.Dispose();
                    serverUserService = null;
                }
            }
            catch { }
        }

        #region IDisposable

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
                Disconnect();
            }

            disposed = true;
        }

        ~LoginPageViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable

        public class UserComboBoxItem
        {
            public UserComboBoxItem(Guid id, string name)
            {
                Id = id;
                Name = name;
            }

            public Guid Id { get; set; }

            public string Name { get; set; }
        }
    }
}