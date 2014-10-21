using Junte.Parallel.Common;
using Junte.WCF.Common;
using Microsoft.Practices.ServiceLocation;
using Queue.Common;
using Queue.Display.Types;
using Queue.Model.Common;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using Queue.UI.WPF.Enums;
using Queue.UI.WPF.Types;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Queue.Display.Models
{
    public class HomePageVM : ObservableObject, IDisposable
    {
        private const int PingInterval = 10000;

        private DispatcherTimer pingTimer;
        private ServerState serverState;

        private readonly ChannelManager<IServerTcpService> channelManager;
        private readonly TaskPool taskPool;
        private bool disposed;
        private Workplace workplace;
        private ServerCallback callbackObject;
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

        public HomePageVM()
        {
            workplace = ServiceLocator.Current.GetInstance<Workplace>();

            WorkplaceTitle = workplace.ToString();
            WorkplaceComment = workplace.Comment;

            channelManager = new ChannelManager<IServerTcpService>(ServiceLocator.Current.GetInstance<DuplexChannelBuilder<IServerTcpService>>());
            taskPool = new TaskPool();

            callbackObject = new ServerCallback();
            callbackObject.OnCurrentClientRequestPlanUpdated += CurrentClientRequestPlanUpdated;

            pingChannel = channelManager.CreateChannel(callbackObject);

            pingTimer = new DispatcherTimer(DispatcherPriority.Background);
            pingTimer.Interval = TimeSpan.FromSeconds(1);
            pingTimer.Tick += PingElapsed;

            currentRequests = new ObservableCollection<ClientRequestWrapper>();
        }

        public void Initialize()
        {
            pingTimer.Start();
        }

        private void UpdateOperatorCurrentRequest(Operator op, ClientRequestPlan plan)
        {
            ClientRequestWrapper wrapper = CurrentRequests.SingleOrDefault(w => w.Operator.Equals(op));
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
            IList<KeyValuePair<Operator, ClientRequestPlan>> plans = (await pingChannel.Service.GetCurrentClientRequestPlans())
                                                                 .Where(p => p.Key != null && p.Key.Workplace.Equals(workplace))
                                                                 .ToList();
            foreach (KeyValuePair<Operator, ClientRequestPlan> plan in plans)
            {
                UpdateOperatorCurrentRequest(plan.Key, plan.Value);
            }

            pingChannel.Service.Subscribe(ServerServiceEventType.CurrentClientRequestPlanUpdated, new ServerSubscribtionArgs
            {
                Operators = await pingChannel.Service.GetWorkplaceOperators(workplace.Id)
            });
        }

        private void CurrentClientRequestPlanUpdated(object sender, ServerEventArgs e)
        {
            if (e.Operator == null)
            {
                return;
            }

            UpdateOperatorCurrentRequest(e.Operator, e.ClientRequestPlan);
        }

        ~HomePageVM()
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
}