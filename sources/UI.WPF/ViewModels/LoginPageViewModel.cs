using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using MahApps.Metro;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.UI.WPF.Pages.ViewModels
{
    public class LoginPageViewModel : ObservableObject, IDisposable
    {
        private LoginPage owner;
        private string endpoint;
        private bool isConnected;
        private string password;
        private bool isRemember;
        private AccentColorComboBoxItem selectedAccent;
        private UserComboBoxItem selectedUser;
        private List<UserComboBoxItem> users;
        private UserRole userRole;

        private ChannelManager<IServerTcpService> channelManager;
        private TaskPool taskPool;
        private Language selectedLanguage;
        private IConfigurationManager configuration;
        private LoginSettings loginSettings;
        private LoginFormSettings loginFormSettings;

        public event EventHandler OnLogined;

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

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
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                CultureInfo.DefaultThreadCurrentCulture = culture;
            }
        }

        public List<UserComboBoxItem> Users
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

        public LoginPageViewModel(UserRole userRole, LoginPage owner)
        {
            this.userRole = userRole;
            this.owner = owner;

            AccentColors = ThemeManager.Accents.Select(a => new AccentColorComboBoxItem(a.Name, a.Resources["AccentColorBrush"] as Brush)).ToArray();

            taskPool = new TaskPool();

            ConnectCommand = new RelayCommand(Connect);
            LoginCommand = new RelayCommand(Login);
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            LoadSettings();

            if (IsRemember || (loginSettings.User != Guid.Empty))
            {
                Connect();
            }
        }

        private void LoadSettings()
        {
            configuration = ServiceLocator.Current.GetInstance<IConfigurationManager>();
            loginSettings = configuration.GetSection<LoginSettings>(LoginSettings.SectionKey, s => s.Endpoint = "net.tcp://queue:4505");
            Endpoint = loginSettings.Endpoint;
            Password = loginSettings.Password;

            loginFormSettings = configuration.GetSection<LoginFormSettings>(LoginFormSettings.SectionKey);
            IsRemember = loginFormSettings.IsRemember;
            SelectedLanguage = loginFormSettings.Language;

            if (!string.IsNullOrWhiteSpace(loginFormSettings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == loginFormSettings.Accent);
            }

            owner.Adjust();
        }

        public async void Connect()
        {
            ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
            channelManager = new ChannelManager<IServerTcpService>(ChannelBuilder);

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = owner.ShowLoading();
                try
                {
                    Users = (await taskPool.AddTask(channel.Service.GetUserLinks(userRole))).Select(u => new UserComboBoxItem()
                    {
                        Id = u.Id,
                        Name = u.ToString()
                    }).ToList();

                    if (loginSettings.User != Guid.Empty)
                    {
                        SelectedUser = Users.SingleOrDefault(u => u.Id == loginSettings.User);
                    }

                    if (SelectedUser == null)
                    {
                        SelectedUser = Users.First();
                    }

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

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = owner.ShowLoading();

                try
                {
                    User = await taskPool.AddTask(channel.Service.UserLogin(SelectedUser.Id, Password));

                    SaveSettings();

                    if (OnLogined != null)
                    {
                        OnLogined(this, new EventArgs());
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
            loginSettings.Password = IsRemember ? Password : string.Empty;
            loginSettings.User = SelectedUser.Id;

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