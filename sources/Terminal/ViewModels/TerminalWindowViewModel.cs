using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.Terminal.Core;
using Queue.Terminal.Enums;
using Queue.UI.WPF.Enums;
using System;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Queue.Terminal.ViewModels
{
    public class TerminalWindowViewModel : ObservableObject, IDisposable
    {
        private const int PingInterval = 10000;

        private bool disposed = false;

        private Timer pingTimer;
        private Timer timeTimer;

        private ClientRequestModel request;
        private DateTime currentDateTime;
        private ServerState serverState;
        private string title;

        [Dependency]
        public ClientRequestModel Model
        {
            get { return request; }
            set { SetProperty(ref request, value); }
        }

        public DateTime CurrentDateTime
        {
            get { return currentDateTime; }
            set { SetProperty(ref currentDateTime, value); }
        }

        public ServerState ServerState
        {
            get { return serverState; }
            set { SetProperty(ref serverState, value); }
        }

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public ICommand HomeCommand { get; set; }

        public ICommand SearchServiceCommand { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public Navigator Navigator { get; set; }

        [Dependency]
        public DefaultConfig DefaultConfig { get; set; }

        public TerminalWindowViewModel()
        {
            HomeCommand = new RelayCommand(Home);
            SearchServiceCommand = new RelayCommand(SearchService);
        }

        public void Initialize()
        {
            request.PropertyChanged += model_PropertyChanged;

            UpdateTitle();
            StartTimers();

            Navigator.Start();
        }

        private void StartTimers()
        {
            pingTimer = new Timer(1000);
            pingTimer.Elapsed += PingElapsed;

            timeTimer = new Timer(1000);
            timeTimer.Elapsed += TimerElapsed;

            pingTimer.Start();
            timeTimer.Start();
        }

        private void Home()
        {
            Navigator.Reset();
        }

        private void SearchService()
        {
            Navigator.ResetState();
            Navigator.SetCurrentPage(PageType.SearchService);
        }

        private void model_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SelectedService":
                    UpdateTitle();
                    break;
            }
        }

        private void UpdateTitle()
        {
            Title = Model.SelectedService == null ? DefaultConfig.QueueName : Model.SelectedService.ToString();
        }

        private async void PingElapsed(object sender, ElapsedEventArgs e)
        {
            pingTimer.Stop();

            if (pingTimer.Interval != PingInterval)
            {
                pingTimer.Interval = PingInterval;
            }

            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    ServerState = ServerState.Request;
                    ServerDateTime.Sync(await TaskPool.AddTask(channel.Service.GetDateTime()));
                    ServerState = ServerState.Available;
                }
                catch
                {
                    ServerState = ServerState.Unavailable;
                }
                finally
                {
                    pingTimer.Start();
                }
            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            CurrentDateTime = ServerDateTime.Now;
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
                    if (pingTimer != null)
                    {
                        pingTimer.Stop();
                        pingTimer.Dispose();
                    }

                    if (timeTimer != null)
                    {
                        timeTimer.Stop();
                        timeTimer.Dispose();
                    }
                }
            }

            disposed = true;
        }

        ~TerminalWindowViewModel()
        {
            Dispose(false);
        }
    }
}