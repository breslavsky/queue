﻿using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace Queue.Notification.ViewModels
{
    public class ClientRequestsControlViewModel : ObservableObject, IDisposable
    {
        private const int DefaultClientRequestsLength = 6;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed = false;

        private TimeSpan ClientRequestTimeout;

        private object updateLock = new object();

        private DispatcherTimer timer;

        private int ClientRequestsLength;
        private AutoRecoverCallbackChannel channel;

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public ClientRequestsStateListener ClientRequestsStateListener { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public ObservableCollection<ClientRequestWrap> Requests { get; set; }

        public ClientRequestsControlViewModel()
        {
            ServiceLocator.Current.GetInstance<UnityContainer>().BuildUp(this);

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            ClientRequestTimeout = TimeSpan.FromMinutes(20);
            ClientRequestsLength = DefaultClientRequestsLength;

            ClientRequestsStateListener.ClientRequestUpdated += ClientRequestUpdated;

            Requests = new ObservableCollection<ClientRequestWrap>();

            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private async void Loaded()
        {
            await ReadConfig();

            channel = new AutoRecoverCallbackChannel(CreateServerCallback(), Subscribe);
        }

        private async Task ReadConfig()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    AdjustConfig(await TaskPool.AddTask(channel.Service.GetNotificationConfig()));
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
            }
        }

        private void AdjustConfig(NotificationConfig config)
        {
            ClientRequestsLength = config.ClientRequestsLength;
        }

        private ServerCallback CreateServerCallback()
        {
            var result = new ServerCallback();
            result.OnConfigUpdated += OnConfigUpdated;

            return result;
        }

        private void OnConfigUpdated(object sender, ServerEventArgs e)
        {
            switch (e.Config.Type)
            {
                case ConfigType.Notification:
                    AdjustConfig(e.Config as NotificationConfig);
                    break;
            }
        }

        private void Subscribe(IServerTcpService service)
        {
            service.Subscribe(ServerServiceEventType.ConfigUpdated, new ServerSubscribtionArgs()
            {
                ConfigTypes = new[] { ConfigType.Notification }
            });
        }

        private void ClientRequestUpdated(object sender, ClientRequest e)
        {
            AdjustClientRequests(e);
        }

        public void AdjustClientRequests(ClientRequest request)
        {
            lock (updateLock)
            {
                var wrap = Requests.SingleOrDefault(r => r.Request.Equals(request));

                bool isImportantState = (request.State == ClientRequestState.Calling)
                                    || (request.State == ClientRequestState.Absence);

                if (!isImportantState && (wrap == null))
                {
                    return;
                }

                if (isImportantState)
                {
                    if (wrap != null)
                    {
                        Requests.Remove(wrap);
                    }

                    Requests.Insert(0, new ClientRequestWrap()
                    {
                        Request = request,
                        Added = DateTime.Now
                    });
                }
                else
                {
                    wrap.Request = request;
                    wrap.Added = DateTime.Now;
                }

                AdjustRequestsLength();
            }
        }

        private void AdjustRequestsLength()
        {
            while (Requests.Count > ClientRequestsLength)
            {
                Requests.RemoveAt(Requests.Count - 1);
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            lock (updateLock)
            {
                var now = DateTime.Now;

                foreach (var item in Requests.Where(r => r.Request.IsClosed && (now - r.Added) > ClientRequestTimeout).ToArray())
                {
                    Requests.Remove(item);
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
                timer.Stop();
            }

            disposed = true;
        }

        ~ClientRequestsControlViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }

    public class ClientRequestWrap : ObservableObject
    {
        private ClientRequest request;

        public ClientRequest Request
        {
            get { return request; }
            set { SetProperty(ref request, value); }
        }

        public DateTime Added { get; set; }
    }
}