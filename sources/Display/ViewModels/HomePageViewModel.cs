﻿using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF.Enums;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Drawing = System.Drawing;

namespace Queue.Display.ViewModels
{
    public class HomePageViewModel : ObservableObject, IDisposable
    {
        private const int PingInterval = 10000;

        private DispatcherTimer pingTimer;
        private ServerState serverState;

        private readonly DuplexChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private bool disposed;
        private Workplace workplace;
        private QueuePlanCallback callbackObject;
        private Channel<IServerTcpService> pingChannel;
        private string workplaceTitle;
        private string workplaceComment;
        private bool showNotification;
        private ObservableCollection<ClientRequestWrapper> currentRequests;

        public ServerState ServerState
        {
            get { return serverState; }
            set { SetProperty(ref serverState, value); }
        }

        public string WorkplaceComment
        {
            get { return workplaceComment; }
            set { SetProperty(ref workplaceComment, value); }
        }

        public string WorkplaceTitle
        {
            get { return workplaceTitle; }
            set { SetProperty(ref workplaceTitle, value); }
        }

        public bool ShowNotification
        {
            get { return showNotification; }
            set { SetProperty(ref showNotification, value); }
        }

        public ObservableCollection<ClientRequestWrapper> CurrentRequests
        {
            get { return currentRequests; }
            set { SetProperty(ref currentRequests, value); }
        }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public HomePageViewModel()
        {
            workplace = ServiceLocator.Current.GetInstance<Workplace>();

            WorkplaceTitle = workplace.ToString();
            WorkplaceComment = workplace.Comment;

            channelManager = new DuplexChannelManager<IServerTcpService>(ServiceLocator.Current.GetInstance<DuplexChannelBuilder<IServerTcpService>>());
            taskPool = new TaskPool();

            callbackObject = new QueuePlanCallback();
            callbackObject.OnCurrentClientRequestPlanUpdated += CurrentClientRequestPlanUpdated;

            pingChannel = channelManager.CreateChannel(callbackObject);

            pingTimer = new DispatcherTimer(DispatcherPriority.Background);
            pingTimer.Interval = TimeSpan.FromSeconds(1);
            pingTimer.Tick += PingElapsed;

            currentRequests = new ObservableCollection<ClientRequestWrapper>();

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void Loaded()
        {
            pingTimer.Start();
        }

        private void Unloaded()
        {
            Dispose();
        }

        private void UpdateOperatorCurrentRequest(Operator op, ClientRequestPlan plan)
        {
            var wrapper = CurrentRequests.SingleOrDefault(w => w.Operator.Equals(op));
            if ((plan == null) && (wrapper == null))
            {
                return;
            }

            if (wrapper != null)
            {
                if (IsActiveRequest(plan))
                {
                    wrapper.Request = plan.ClientRequest;
                }
                else
                {
                    CurrentRequests.Remove(wrapper);
                }
            }
            else
            {
                if (IsActiveRequest(plan))
                {
                    CurrentRequests.Add(new ClientRequestWrapper()
                    {
                        Request = plan.ClientRequest,
                        Operator = op
                    });
                }
            }
            ShowNotification = CurrentRequests.Count > 0;
        }

        private bool IsActiveRequest(ClientRequestPlan plan)
        {
            return (plan != null) && (plan.ClientRequest != null) &&
                ((plan.ClientRequest.State == ClientRequestState.Calling) || (plan.ClientRequest.State == ClientRequestState.Rendering));
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

                if (!pingChannel.IsConnected)
                {
                    await Subscribe();
                }

                ServerDateTime.Sync(await taskPool.AddTask(pingChannel.Service.GetDateTime()));

                ServerState = ServerState.Available;
            }
            catch
            {
                ServerState = ServerState.Unavailable;

                pingChannel.Dispose();
                pingChannel = channelManager.CreateChannel(callbackObject);
            }

            pingTimer.Start();
        }

        private async Task Subscribe()
        {
            var plans = (await pingChannel.Service.GetCurrentClientRequestPlans())
                                                .Where(p => p.Key != null && p.Key.Workplace.Equals(workplace))
                                                .ToList();
            foreach (var plan in plans)
            {
                UpdateOperatorCurrentRequest(plan.Key, plan.Value);
            }

            pingChannel.Service.Subscribe(QueuePlanEventType.CurrentClientRequestPlanUpdated, new QueuePlanSubscribtionArgs
            {
                Operators = await pingChannel.Service.GetWorkplaceOperators(workplace.Id)
            });
        }

        private void CurrentClientRequestPlanUpdated(object sender, QueuePlanEventArgs e)
        {
            if (e.Operator == null)
            {
                return;
            }

            UpdateOperatorCurrentRequest(e.Operator, e.ClientRequestPlan);
        }

        ~HomePageViewModel()
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
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                if (pingTimer != null)
                {
                    pingTimer.Tick -= PingElapsed;
                    pingTimer.Stop();
                }
                if (taskPool != null)
                {
                    taskPool.Dispose();
                }
                if (pingChannel != null)
                {
                    pingChannel.Dispose();
                }
                if (channelManager != null)
                {
                    channelManager.Dispose();
                }
            }
            disposed = true;
        }
    }

    public class ClientRequestWrapper : ObservableObject
    {
        private ClientRequest request;

        private string state;
        private Brush stateBrush;

        public int Number { get; set; }

        public Operator Operator { get; set; }

        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        public Brush StateBrush
        {
            get { return stateBrush; }
            set { SetProperty(ref stateBrush, value); }
        }

        public ClientRequest Request
        {
            get { return request; }
            set
            {
                request = value;
                Update();
            }
        }

        private void Update()
        {
            Number = request.Number;
            State = Translater.Enum(request.State);

            Drawing.Color c = Drawing.ColorTranslator.FromHtml(request.Color);
            StateBrush = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));
        }
    }
}