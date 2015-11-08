using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Contracts.Server;
using Queue.UI.WPF.Enums;
using System;
using System.Timers;
using System.Windows.Input;

namespace Queue.UI.WPF.ViewModels
{
    public class ConnectionStatusControlViewModel : RichViewModel, IDisposable
    {
        private const int PingInterval = 10000;

        private bool disposed;

        private Timer pingTimer;
        private Timer timeTimer;
        private Channel<IServerTcpService> channel;

        private ServerState serverState;
        private DateTime currentDateTime;
        private string currentDateTimeText;

        public ServerState ServerState
        {
            get { return serverState; }
            set { SetProperty(ref serverState, value); }
        }

        public DateTime CurrentDateTime
        {
            get { return currentDateTime; }
            set
            {
                SetProperty(ref currentDateTime, value);
                CurrentDateTimeText = value.ToString("dd MMMM yyyy (dddd) HH:mm:ss");
            }
        }

        public string CurrentDateTimeText
        {
            get { return currentDateTimeText; }
            set { SetProperty(ref currentDateTimeText, value); }
        }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public ConnectionStatusControlViewModel()
            : base()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            InitTimers();
        }

        private void Loaded()
        {
            channel = ChannelManager.CreateChannel();

            pingTimer.Start();
            timeTimer.Start();
        }

        private void Unloaded()
        {
            Dispose();
        }

        private void InitTimers()
        {
            pingTimer = new Timer();
            pingTimer.Interval = 1000;
            pingTimer.Elapsed += PingElapsed;

            timeTimer = new Timer();
            timeTimer.Interval = 1000;
            timeTimer.Elapsed += TimerElapsed;
        }

        private async void PingElapsed(object sender, EventArgs e)
        {
            pingTimer.Stop();
            if (pingTimer.Interval < PingInterval)
            {
                pingTimer.Interval = PingInterval;
            }

            try
            {
                ServerState = ServerState.Request;

                ServerDateTime.Sync(await channel.Service.GetDateTime());

                ServerState = ServerState.Available;
            }
            catch
            {
                ServerState = ServerState.Unavailable;

                channel.Dispose();
                channel = ChannelManager.CreateChannel();
            }

            pingTimer.Start();
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            CurrentDateTime = ServerDateTime.Now;
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
                try
                {
                    if (channel != null)
                    {
                        channel.Dispose();
                        channel = null;
                    }
                }
                catch { }
            }

            disposed = true;
        }

        private void DestroyTimers()
        {
            pingTimer.Stop();
            pingTimer.Elapsed -= PingElapsed;
            pingTimer = null;

            timeTimer.Stop();
            timeTimer.Elapsed -= TimerElapsed;
            timeTimer = null;
        }

        ~ConnectionStatusControlViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}