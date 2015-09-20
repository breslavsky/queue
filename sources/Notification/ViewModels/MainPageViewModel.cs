using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Sounds;
using Queue.UI.WPF.Types;
using System;
using System.Media;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Queue.Notification.ViewModels
{
    public class MainPageViewModel : ObservableObject, IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed = false;
        private object voiceLock;

        public ClientRequest[] callingClientRequests;

        private AutoRecoverCallbackChannel channel;

        public CallClientControlViewModel CallClientModel { get; set; }

        public event EventHandler<ClientRequest> RequestUpdated = delegate { };

        public event EventHandler<int> RequestsLengthChanged = delegate { };

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        public MainPageViewModel()
        {
            voiceLock = new object();

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            CallClientModel = new CallClientControlViewModel();
        }

        private async void Loaded()
        {
            await ReadConfigs();

            channel = new AutoRecoverCallbackChannel(CreateServerCallback(), Subscribe);
        }

        private async Task ReadConfigs()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                var loading = Window.ShowLoading();
                try
                {
                    ReadNotificationConfig(await TaskPool.AddTask(channel.Service.GetNotificationConfig()));
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

        private ServerCallback CreateServerCallback()
        {
            var result = new ServerCallback();
            result.OnClientRequestUpdated += OnClientRequestUpdated;
            result.OnCallClient += OnCallClient;
            result.OnConfigUpdated += OnConfigUpdated;

            return result;
        }

        private void Subscribe(IServerTcpService service)
        {
            service.Subscribe(ServerServiceEventType.ClientRequestUpdated);
            service.Subscribe(ServerServiceEventType.CallClient);
            service.Subscribe(ServerServiceEventType.ConfigUpdated, new ServerSubscribtionArgs()
            {
                ConfigTypes = new[] { ConfigType.Notification }
            });
        }

        private void OnConfigUpdated(object sender, ServerEventArgs e)
        {
            switch (e.Config.Type)
            {
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

        private void ReadNotificationConfig(NotificationConfig notificationConfig)
        {
            RequestsLengthChanged(this, notificationConfig.ClientRequestsLength);
        }

        private void NotifyClientRequestUpdated(ClientRequest request)
        {
            if (request != null)
            {
                RequestUpdated(this, request);
            }
        }

        private void CallClient(ClientRequest request)
        {
            lock (voiceLock)
            {
                CallClientModel.ShowMessage(request);

                PlayVoice(request);
                CallClientModel.CloseMessage();
            }
        }

        private void PlayVoice(ClientRequest request)
        {
            using (var soundPlayer = new SoundPlayer())
            {
                soundPlayer.PlayStream(Tones.Notify);
                soundPlayer.PlayStream(Words.Number);

                soundPlayer.PlayNumber(request.Number);

                var workplace = request.Operator.Workplace;
                soundPlayer.PlayStream(Workplaces.ResourceManager.GetStream(workplace.Type.ToString()));
                soundPlayer.PlayNumber(workplace.Number);

                if (workplace.Modificator != WorkplaceModificator.None)
                {
                    soundPlayer.PlayStream(Workplaces.ResourceManager.GetStream(workplace.Modificator.ToString()));
                }
            }
        }

        private void Unloaded()
        {
            Dispose();
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
                if (channel != null)
                {
                    channel.Dispose();
                    channel = null;
                }

                if (TaskPool != null)
                {
                    TaskPool.Cancel();
                    TaskPool.Dispose();
                    TaskPool = null;
                }
            }

            disposed = true;
        }

        ~MainPageViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}