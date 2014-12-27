using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.UI.WPF.Types;
using Junte.WCF.Common;
using MahApps.Metro;
using Queue.Display.Types;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF;
using Queue.UI.WPF.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Queue.Display.Models
{
    public class LoginPageVM : ObservableObject, IDisposable
    {
        private bool disposed;
        private readonly TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;
        private AccentColorComboBoxItem selectedAccent;
        private bool isConnected;

        private string endpoint;
        private bool isRemember;
        private RichPage owner;
        private LoginSettings settings;
        private IdentifiedEntity[] workplaces;
        private Guid selectedWorkplace;

        private Lazy<ICommand> connectCommand;
        private Lazy<ICommand> loginCommand;

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

        public IdentifiedEntity[] Workplaces
        {
            get { return workplaces; }
            set { SetProperty(ref workplaces, value); }
        }

        public Guid SelectedWorkplace
        {
            get { return selectedWorkplace; }
            set { SetProperty(ref selectedWorkplace, value); }
        }

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

        public ICommand ConnectCommand { get { return connectCommand.Value; } }

        public ICommand LoginCommand { get { return loginCommand.Value; } }

        public Workplace Workplace { get; private set; }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        public event EventHandler OnLogined;

        public LoginPageVM(RichPage owner)
        {
            this.owner = owner;

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
        }

        public void ApplySettings(LoginSettings settings)
        {
            this.settings = settings;

            Endpoint = settings.Endpoint;
            IsRemember = settings.IsRemember;

            if (!string.IsNullOrWhiteSpace(settings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == settings.Accent);
            }
        }

        public void Initialize()
        {
            if (IsRemember)
            {
                Connect();
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

            using (Channel<IServerTcpService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = owner.ShowLoading();

                try
                {
                    Workplaces = await taskPool.AddTask(channel.Service.GetWorkplacesLinks());

                    SelectedWorkplace = settings != null && settings.WorkplaceId != Guid.Empty
                        ? settings.WorkplaceId : Workplaces.First().Id;

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

            LoadingControl loading = owner.ShowLoading();

            using (var channel = channelManager.CreateChannel())
            {
                try
                {
                    Workplace = await taskPool.AddTask(channel.Service.GetWorkplace(SelectedWorkplace));

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

        ~LoginPageVM()
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
    }
}