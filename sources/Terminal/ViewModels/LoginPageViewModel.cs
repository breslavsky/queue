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
using Queue.UI.WPF.Types;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;
using WinForms = System.Windows.Forms;

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
        private ScreenNumberItem selectedScreenNumber;
        private UserComboBoxItem selectedUser;
        private UserComboBoxItem[] users;
        private UserRole userRole;
        private Language selectedLanguage;

        private ChannelManager<IUserTcpService> channelManager;
        private UserService serverUserService;

        public event EventHandler OnLogined = delegate { };

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

        [Dependency]
        public IMainWindow Window { get; set; }

        [Dependency]
        public ConfigurationManager ConfigurationManager { get; set; }

        [Dependency]
        public AppSettings AppSettings { get; set; }

        public LoginPageViewModel(UserRole userRole, LoginPage owner) :
            base()
        {
            this.userRole = userRole;
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();
            ScreensNumbers = WinForms.Screen.AllScreens.Select((s, pos) => new ScreenNumberItem((byte)pos)).ToArray();

            ConnectCommand = new RelayCommand(Connect);
            LoginCommand = new RelayCommand(Login);
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            LoadSettings();

            if (IsRemember || (AppSettings.User != Guid.Empty))
            {
                Connect();
            }
        }

        private void LoadSettings()
        {
            Endpoint = AppSettings.Endpoint;
            Password = AppSettings.Password;
            IsRemember = AppSettings.IsRemember;
            SelectedLanguage = AppSettings.Language;
            SelectedScreenNumber = ScreensNumbers.FirstOrDefault(d => d.Number == AppSettings.ScreenNumber);

            if (!String.IsNullOrWhiteSpace(AppSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == AppSettings.Accent);
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

            if (AppSettings.User != Guid.Empty)
            {
                SelectedUser = Users.SingleOrDefault(u => u.Id == AppSettings.User);
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
            AppSettings.Endpoint = Endpoint;
            AppSettings.Password = IsRemember ? Password : String.Empty;
            AppSettings.User = SelectedUser.Id;
            AppSettings.IsRemember = IsRemember;
            AppSettings.Accent = SelectedAccent == null ? String.Empty : SelectedAccent.Name;
            AppSettings.Language = SelectedLanguage;
            AppSettings.ScreenNumber = SelectedScreenNumber == null ? (byte)0 : SelectedScreenNumber.Number;

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