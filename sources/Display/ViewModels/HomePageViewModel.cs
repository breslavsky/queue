using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Model.Common;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WPF;
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
    public class HomePageViewModel : RichViewModel, IDisposable
    {
        private const int PingInterval = 10000;

        private DispatcherTimer pingTimer;
        private ServerState serverState;

        private bool disposed;
        private QueuePlanCallback callbackObject;
        private Channel<IQueuePlanTcpService> queuePlanChannel;
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

        [Dependency]
        public DuplexChannelManager<IQueuePlanTcpService> QueuePlanChannelManager { get; set; }

        [Dependency]
        public ChannelManager<IWorkplaceTcpService> WorkplaceChannelManager { get; set; }

        [Dependency]
        public Workplace Workplace { get; set; }

        public HomePageViewModel() :
            base()
        {
            WorkplaceTitle = Workplace.ToString();
            WorkplaceComment = Workplace.Comment;

            callbackObject = new QueuePlanCallback();
            callbackObject.OnCurrentClientRequestPlanUpdated += CurrentClientRequestPlanUpdated;

            queuePlanChannel = QueuePlanChannelManager.CreateChannel(callbackObject);

            BuildUpTimer();

            currentRequests = new ObservableCollection<ClientRequestWrapper>();

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);
        }

        private void BuildUpTimer()
        {
            pingTimer = new DispatcherTimer(DispatcherPriority.Background);
            pingTimer.Interval = TimeSpan.FromSeconds(1);
            pingTimer.Tick += PingElapsed;
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

                if (!queuePlanChannel.IsConnected)
                {
                    await Subscribe();
                }

                await queuePlanChannel.Service.Heartbeat();

                ServerState = ServerState.Available;
            }
            catch
            {
                ServerState = ServerState.Unavailable;

                CloseChannel();

                queuePlanChannel = QueuePlanChannelManager.CreateChannel(callbackObject);
            }

            pingTimer.Start();
        }

        private async Task Subscribe()
        {
            var plans = (await queuePlanChannel.Service.GetCurrentClientRequestPlans())
                                                .Where(p => p.Key != null && p.Key.Workplace.Equals(Workplace))
                                                .ToList();
            foreach (var plan in plans)
            {
                UpdateOperatorCurrentRequest(plan.Key, plan.Value);
            }

            using (var workplaceChannel = WorkplaceChannelManager.CreateChannel())
            {
                queuePlanChannel.Service.Subscribe(QueuePlanEventType.CurrentClientRequestPlanUpdated, new QueuePlanSubscribtionArgs
                {
                    Operators = await workplaceChannel.Service.GetWorkplaceOperators(Workplace.Id)
                });
            }
        }

        private void CurrentClientRequestPlanUpdated(object sender, QueuePlanEventArgs e)
        {
            if (e.Operator == null)
            {
                return;
            }

            UpdateOperatorCurrentRequest(e.Operator, e.ClientRequestPlan);
        }

        private void CloseChannel()
        {
            if (queuePlanChannel != null)
            {
                queuePlanChannel.Dispose();
            }

            queuePlanChannel = null;
        }

        #region IDisposable

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
                try
                {
                    CloseChannel();

                    QueuePlanChannelManager.Dispose();
                    WorkplaceChannelManager.Dispose();

                    if (pingTimer != null)
                    {
                        pingTimer.Tick -= PingElapsed;
                        pingTimer.Stop();
                        pingTimer = null;
                    }
                }
                catch { }
            }
            disposed = true;
        }

        #endregion IDisposable
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