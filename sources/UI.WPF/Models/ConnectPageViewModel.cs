using Junte.Parallel.Common;
using Junte.WCF.Common;
using MahApps.Metro;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.UI.WPF.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Queue.UI.WPF.Models
{
    public class ConnectPageViewModel : ObservableObject, IDisposable
    {
        private RichPage owner;
        private bool isRemember;
        private string endpoint;
        private AccentColorComboBoxItem selectedAccent;

        private TaskPool taskPool;
        private ChannelManager<IServerTcpService> channelManager;

        private Lazy<ICommand> connectCommand;

        private UserLoginSettings settings;

        public ConnectPageViewModel(RichPage owner)
        {
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
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        public event EventHandler OnConnected;

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

        public ICommand ConnectCommand { get { return this.connectCommand.Value; } }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public DuplexChannelBuilder<IServerTcpService> ChannelBuilder { get; private set; }

        public void ApplyUserSettings(UserLoginSettings settings)
        {
            this.settings = settings;

            Endpoint = settings.Endpoint;
            IsRemember = settings.IsRemember;

            if (!string.IsNullOrWhiteSpace(settings.Accent))
            {
                SelectedAccent = AccentColors.SingleOrDefault(c => c.Name == settings.Accent);
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

        private void Connect()
        {
            ChannelBuilder = new DuplexChannelBuilder<IServerTcpService>(new ServerCallback(), Bindings.NetTcpBinding, new EndpointAddress(Endpoint));
            channelManager = new ChannelManager<IServerTcpService>(ChannelBuilder);

            if (OnConnected != null)
            {
                OnConnected(this, null);
            }
        }
    }
}