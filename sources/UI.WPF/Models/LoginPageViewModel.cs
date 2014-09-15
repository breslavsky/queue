using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using MahApps.Metro;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Queue.UI.WPF.Pages.Models
{
    public class LoginPageViewModel : ObservableObject, IDisposable
    {
        private RichPage owner;
        private string endpoint;
        private bool isConnected;
        private string password;
        private bool isRemember;
        private AccentColorComboBoxItem selectedAccent;
        private UserComboBoxItem selectedUser;
        private List<UserComboBoxItem> users;
        private UserRole userRole;

        private Lazy<ICommand> connectCommand;
        private Lazy<ICommand> loginCommand;

        private ChannelManager<IServerService> channelManager;
        private TaskPool taskPool;

        private UserLoginSettings settings;

        public LoginPageViewModel(UserRole userRole, RichPage owner)
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
            taskPool = new TaskPool();

            connectCommand = new Lazy<ICommand>(() => new RelayCommand(Connect));
            loginCommand = new Lazy<ICommand>(() => new RelayCommand(Login));
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        public event EventHandler OnLogined;

        public DuplexChannelBuilder<IServerService> ChannelBuilder { get; private set; }

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

        public ICommand ConnectCommand { get { return connectCommand.Value; } }

        public ICommand LoginCommand { get { return loginCommand.Value; } }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        #endregion UIProperties

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
        }

        public async void Connect()
        {
            ChannelBuilder = new DuplexChannelBuilder<IServerService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
            channelManager = new ChannelManager<IServerService>(ChannelBuilder);

            using (var channel = channelManager.CreateChannel())
            {
                LoadingControl loading = owner.ShowLoading();
                try
                {
                    Users = (await taskPool.AddTask(channel.Service.GetUserList(userRole))).Select(u => new UserComboBoxItem()
                    {
                        Id = u.Key,
                        Name = u.Value
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

            using (Channel<IServerService> channel = channelManager.CreateChannel())
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