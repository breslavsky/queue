using Junte.Parallel;
using Junte.Translation;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Notification.Settings;
using Queue.Services.Contracts.Hub;
using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Drawing = System.Drawing;

namespace Queue.Notification.ViewModels
{
    public class ClientRequestsControlViewModel : RichViewModel, IDisposable
    {
        private const int DefaultClientRequestsLength = 6;

        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed = false;

        private TimeSpan ClientRequestTimeout;

        private object updateLock = new object();

        private DispatcherTimer timer;

        private int ClientRequestsLength;

        [Dependency]
        public ChannelManager<IServerTcpService> ServerChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        [Dependency]
        public NotificationSettings AppSettings { get; set; }

        [Dependency]
        public HubSettings HubSettings { get; set; }

        [Dependency]
        public ChannelManager<IDisplayTcpService> DisplayChannelManager { get; set; }

        [Dependency]
        public ClientRequestsListener ClientRequestsListener { get; set; }

        [Dependency]
        public IMainWindow Window { get; set; }

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public ObservableCollection<ClientRequestWrap> Requests { get; set; }

        public ClientRequestsControlViewModel()
            : base()
        {
            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            ClientRequestTimeout = TimeSpan.FromMinutes(20);
            ClientRequestsLength = DefaultClientRequestsLength;

            ClientRequestsListener.ClientRequestUpdated += ClientRequestUpdated;

            Requests = new ObservableCollection<ClientRequestWrap>();

            timer = new DispatcherTimer(DispatcherPriority.Background);
            timer.Interval = TimeSpan.FromMinutes(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private async void Loaded()
        {
            try
            {
                await ReadConfig();
            }
            catch (Exception e)
            {
                UIHelper.Warning(null, e.Message);
            }
        }

        private async Task ReadConfig()
        {
            using (var channel = ServerChannelManager.CreateChannel())
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
                    throw new QueueException(exception.Reason.ToString());
                }
                catch (Exception exception)
                {
                    throw new QueueException(exception.Message);
                }
            }
        }

        private void AdjustConfig(NotificationConfig config)
        {
            ClientRequestsLength = config.ClientRequestsLength;
        }

        private void ClientRequestUpdated(object sender, ClientRequest e)
        {
            AdjustClientRequests(e);
        }

        public void AdjustClientRequests(ClientRequest request)
        {
            lock (updateLock)
            {
                Window.Invoke(() =>
                   {
                       var wrap = Requests.SingleOrDefault(r => r.Request.Equals(request));
                       var isImportantState = (request.State == ClientRequestState.Calling) ||
                                               (request.State == ClientRequestState.Absence);

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
                   });
            }

            SendRequestsToDisplays();
        }

        private void AdjustRequestsLength()
        {
            while (Requests.Count > ClientRequestsLength)
            {
                Requests.RemoveAt(Requests.Count - 1);
            }
        }

        private void SendRequestsToDisplays()
        {
            logger.Debug("SendRequestsToDisplays...");
            if (AppSettings.Displays.Count == 0 || !HubSettings.Enabled)
            {
                return;
            }

            foreach (DisplayConfig display in AppSettings.Displays)
            {
                SendRequestsToDisplay(display);
            }
        }

        private async void SendRequestsToDisplay(DisplayConfig display)
        {
            try
            {
                var lines = GetLinesForDisplay(display);

                logger.Debug("show lines [device: {0}; lines: {1}]",
                    display.DeviceId,
                    String.Join(", ", lines.Select(l => String.Format("[{0}: {1}]", l[0], l[1]))));

                using (var channel = DisplayChannelManager.CreateChannel())
                {
                    await channel.Service.ShowLines(display.DeviceId, lines);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private ushort[][] GetLinesForDisplay(DisplayConfig display)
        {
            var toSend = new List<ushort[]>();
            var workplaces = display.Workplaces
                                    .Cast<WorkplaceConfig>()
                                    .Select(c => c.Number);
            logger.Debug("workplaces: " + String.Join(", ", workplaces));

            foreach (var req in Requests)
            {
                logger.Debug("req.Request.Operator.Workplace.Number: " + req.Request.Operator.Workplace.Number);
                if (display.Workplaces.Count > 0 && !workplaces.Contains(req.Request.Operator.Workplace.Number))
                {
                    continue;
                }

                toSend.Add(new[] { (ushort)req.Request.Number, (ushort)req.Request.Operator.Workplace.Number });
            }

            logger.Debug("GetLinesForDisplay: " + toSend.Count);

            return toSend.ToArray();
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

            try
            {
                if (disposing)
                {
                    timer.Stop();
                    timer.Tick -= timer_Tick;
                    timer = null;
                }
            }
            catch { }

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
        private string state;
        private Brush brush;

        public ClientRequest Request
        {
            get { return request; }
            set
            {
                SetProperty(ref request, null);
                SetProperty(ref request, value);
                Adjust();
            }
        }

        public string State
        {
            get { return state; }
            set { SetProperty(ref state, value); }
        }

        public Brush StateBrush
        {
            get { return brush; }
            set { SetProperty(ref brush, value); }
        }

        public DateTime Added { get; set; }

        private void Adjust()
        {
            State = Translater.Enum(request.State);

            var c = Drawing.ColorTranslator.FromHtml(request.Color);
            StateBrush = new SolidColorBrush(Color.FromRgb(c.R, c.G, c.B));
        }
    }
}