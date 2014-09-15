using Junte.Parallel.Common;
using Junte.UI.WPF;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Notification.Types;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Sounds;
using Queue.UI.WPF;
using Queue.UI.WPF.Enums;
using Queue.UI.WPF.Types;
using System;
using System.Media;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Threading;
using Vlc.DotNet.Core.Medias;
using Vlc.DotNet.Wpf;

namespace Queue.Notification.Models
{
    public class HomePageVM : ObservableObject, IDisposable
    {
        private const int PingInterval = 10000;
        private const string MediaFileUriPattern = "{0}/media-config/files/{1}/load";

        private bool disposed = false;

        private ITicker ticker;
        private IRichPage screen;
        private ServerState serverState;
        private DateTime currentDateTime;
        private string currentDateTimeText;
        public ClientRequest[] callingClientRequests;

        private ChannelManager<IServerService> channelManager;
        private Channel<IServerService> callbackChannel;
        private ServerCallback callbackObject;
        private TaskPool taskPool;
        private VlcControl vlcControl;

        private DispatcherTimer pingTimer;
        private DispatcherTimer timeTimer;

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

        public CallClientUserControlVM CallClientModel { get; set; }

        public event EventHandler<ClientRequest> RequestUpdated;

        public event EventHandler<int> RequestsLengthChanged;

        public HomePageVM()
        {
            this.channelManager = ServiceLocator.Current.GetInstance<ChannelManager<IServerService>>();
            this.taskPool = ServiceLocator.Current.GetInstance<TaskPool>();
            this.screen = ServiceLocator.Current.GetInstance<IRichPage>();
            this.ticker = ServiceLocator.Current.GetInstance<ITicker>();

            CallClientModel = new CallClientUserControlVM();

            InitCallbackChannel();
            InitTimers();
        }

        public async void Initialize(VlcControl vlcControl)
        {
            this.vlcControl = vlcControl;

            using (Channel<IServerService> channel = channelManager.CreateChannel())
            {
                LoadingControl loading = screen.ShowLoading();

                try
                {
                    ReadMediaConfig(await taskPool.AddTask(channel.Service.GetMediaConfig()));
                    ReadNotificationConfig(await taskPool.AddTask(channel.Service.GetNotificationConfig()));
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

            pingTimer.Start();
            timeTimer.Start();
        }

        private void InitCallbackChannel()
        {
            callbackObject = CreateServerCallback();
            callbackChannel = channelManager.CreateChannel(callbackObject);
        }

        private ServerCallback CreateServerCallback()
        {
            ServerCallback result = new ServerCallback();
            result.OnClientRequestUpdated += OnClientRequestUpdated;
            result.OnCallClient += OnCallClient;
            result.OnConfigUpdated += OnConfigUpdated;

            return result;
        }

        private void InitTimers()
        {
            pingTimer = new DispatcherTimer(DispatcherPriority.Background);
            pingTimer.Interval = TimeSpan.FromSeconds(1);
            pingTimer.Tick += PingElapsed;

            timeTimer = new DispatcherTimer(DispatcherPriority.Background);
            timeTimer.Interval = TimeSpan.FromSeconds(1);
            timeTimer.Tick += TimerElapsed;
        }

        private async void PingElapsed(object sender, EventArgs e)
        {
            pingTimer.Stop();
            if (pingTimer.Interval.TotalMilliseconds < PingInterval)
            {
                pingTimer.Interval = TimeSpan.FromMilliseconds(PingInterval);
            }

            try
            {
                ServerState = ServerState.Request;

                if (!callbackChannel.IsConnected)
                {
                    Subscribe();
                }

                ServerDateTime.Sync(await taskPool.AddTask(callbackChannel.Service.GetDateTime()));

                ServerState = ServerState.Available;
            }
            catch
            {
                ServerState = ServerState.Unavailable;

                callbackChannel.Dispose();
                callbackChannel = channelManager.CreateChannel(callbackObject);
            }

            pingTimer.Start();
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            CurrentDateTime = ServerDateTime.Now;
        }

        private void OnConfigUpdated(object sender, ServerEventArgs e)
        {
            switch (e.Config.Type)
            {
                case ConfigType.Media:
                    ReadMediaConfig(e.Config as MediaConfig);
                    break;

                case ConfigType.Notification:
                    ReadNotificationConfig(e.Config as NotificationConfig);
                    break;
            }
        }

        private void OnClientRequestUpdated(object sender, ServerEventArgs e)
        {
            Task.Run(() => NotifyClientRequestUpdated(e.ClientRequest));
        }

        public void OnCallClient(object sender, ServerEventArgs e)
        {
            Task.Run(() =>
            {
                NotifyClientRequestUpdated(e.ClientRequest);
                CallClient(e.ClientRequest);
            });
        }

        private void Subscribe()
        {
            callbackChannel.Service.Subscribe(ServerServiceEventType.ClientRequestUpdated);
            callbackChannel.Service.Subscribe(ServerServiceEventType.CallClient);
            callbackChannel.Service.Subscribe(ServerServiceEventType.ConfigUpdated, new ServerSubscribtionArgs()
            {
                ConfigTypes = new ConfigType[] { ConfigType.Media, ConfigType.Notification }
            });
        }

        private void ReadNotificationConfig(NotificationConfig notificationConfig)
        {
            if (RequestsLengthChanged != null)
            {
                RequestsLengthChanged(this, notificationConfig.ClientRequestsLength);
            }
        }

        private void ReadMediaConfig(MediaConfig mediaConfig)
        {
            ticker.SetTicker(mediaConfig.Ticker);
            ticker.SetSpeed(mediaConfig.TickerSpeed);
            ticker.Start();

            foreach (MediaConfigFile f in mediaConfig.MediaFiles)
            {
                vlcControl.Medias.Add(new LocationMedia(string.Format(MediaFileUriPattern, mediaConfig.ServiceUrl, f.Id)));
            }

            vlcControl.Stop();
            vlcControl.Play();
        }

        private void NotifyClientRequestUpdated(ClientRequest request)
        {
            if ((request != null) && (RequestUpdated != null))
            {
                RequestUpdated(this, request);
            }
        }

        private void CallClient(ClientRequest request)
        {
            lock (typeof(SoundPlayer))
            {
                CallClientModel.ShowMessage(request);
                PlayVoice(request);
                CallClientModel.CloseMessage();
            }
        }

        private void PlayVoice(ClientRequest request)
        {
            using (SoundPlayer soundPlayer = new SoundPlayer())
            {
                soundPlayer.PlayStream(Tones.Notify);
                soundPlayer.PlayStream(Words.Number);

                soundPlayer.PlayNumber(request.Number);

                Workplace workplace = request.Operator.Workplace;
                soundPlayer.PlayStream(Workplaces.ResourceManager.GetStream(workplace.Type.ToString()));
                soundPlayer.PlayNumber(workplace.Number);

                if (workplace.Modificator != WorkplaceModificator.None)
                {
                    soundPlayer.PlayStream(Workplaces.ResourceManager.GetStream(workplace.Modificator.ToString()));
                }
            }
        }

        #region IDisposable

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
                    if (pingTimer != null)
                    {
                        pingTimer.Stop();
                    }

                    if (timeTimer != null)
                    {
                        timeTimer.Stop();
                    }

                    if (callbackChannel != null)
                    {
                        callbackChannel.Dispose();
                    }
                }
            }

            disposed = true;
        }

        ~HomePageVM()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}