using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using MahApps.Metro;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using WPFLocalizeExtension.Engine;

namespace Queue.UI.WPF.Pages.Models
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

        private UserLoginSettings settings;

        public event EventHandler OnLogined;

        private EnumItem<Language> selectedLanguage;

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

        public List<AccentColorComboBoxItem> AccentColors { get; set; }

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

        public EnumItem<Language>[] Languages { get; set; }

        public EnumItem<Language> SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                SetProperty(ref selectedLanguage, value);

                LocalizeDictionary.Instance.SetCurrentThreadCulture = true;
                LocalizeDictionary.Instance.Culture = SelectedLanguage.Value.GetCulture();
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

            Endpoint = "net.tcp://queue:4505";
            AccentColors = ThemeManager.Accents
                                          .Select(a => new AccentColorComboBoxItem()
                                          {
                                              Name = a.Name,
                                              ColorBrush = a.Resources["AccentColorBrush"] as Brush
                                          })
                                          .ToList();

            Languages = EnumItem<Language>.GetItems();
            SelectedLanguage = Languages[0];

            taskPool = new TaskPool();

            ConnectCommand = new RelayCommand(Connect);
            LoginCommand = new RelayCommand(Login);
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        public void ApplyUserSettings(UserLoginSettings settings)
        {
            this.settings = settings;

            Endpoint = settings.Endpoint;
            Password = settings.Password;
            IsRemember = settings.IsRemember;

            if (!string.IsNullOrWhiteSpace(settings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == settings.Accent);
            }

            owner.AdjustModel();
        }

        public async void Connect()
        {
            ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
            channelManager = new ChannelManager<IServerTcpService>(ChannelBuilder);

            using (var channel = channelManager.CreateChannel())
            {
                LoadingControl loading = owner.ShowLoading();
                try
                {
                    Users = (await taskPool.AddTask(channel.Service.GetUserLinks(userRole))).Select(u => new UserComboBoxItem()
                    {
                        Id = u.Id,
                        Name = u.ToString()
                    }).ToList();

                    if (settings != null && settings.UserId != Guid.Empty)
                    {
                        SelectedUser = Users.SingleOrDefault(u => u.Id == settings.UserId);
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

        private void Unloaded()
        {
            Dispose();
        }

        private void Loaded()
        {
            if (IsRemember)
            {
                Connect();
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
    }
}