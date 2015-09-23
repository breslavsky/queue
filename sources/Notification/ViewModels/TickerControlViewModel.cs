using Junte.Parallel;
using Junte.UI.WPF;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Common;
using Queue.Model.Common;
using Queue.Notification.UserControls;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Queue.Notification.ViewModels
{
    public class TickerControlViewModel : ObservableObject, IDisposable
    {
        private const double MillisecondsPerUnit = 15;
        private const int DefaultSpeed = 5;

        private bool disposed;

        private string ticker;
        public bool isRunning;

        private string newTicker;
        private double speed;

        private TranslateTransform translateTransform;
        private TickerControl control;
        private AutoRecoverCallbackChannel channel;

        public ICommand LoadedCommand { get; set; }

        public ICommand UnloadedCommand { get; set; }

        public string Ticker
        {
            get { return ticker; }
            set { SetProperty(ref ticker, value); }
        }

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        public TickerControlViewModel(TickerControl control)
        {
            this.control = control;
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

            LoadedCommand = new RelayCommand(Loaded);
            UnloadedCommand = new RelayCommand(Unloaded);

            translateTransform = new TranslateTransform();
        }

        private async void Loaded()
        {
            try
            {
                control.TickerItem.RenderTransform = translateTransform;

                await ReadConfig();

                channel = new AutoRecoverCallbackChannel(CreateServerCallback(), Subscribe);
            }
            catch (Exception ex)
            {
                UIHelper.Warning(null, String.Format("Ошибка инициализации бегущей строки: {0}", ex.Message));
            }
        }

        private async Task ReadConfig()
        {
            using (var channel = ChannelManager.CreateChannel())
            {
                try
                {
                    ApplyConfig(await TaskPool.AddTask(channel.Service.GetMediaConfig()));
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

        private ServerCallback CreateServerCallback()
        {
            var result = new ServerCallback();
            result.OnConfigUpdated += OnConfigUpdated;

            return result;
        }

        private void Subscribe(IServerTcpService service)
        {
            service.Subscribe(ServerServiceEventType.ConfigUpdated, new ServerSubscribtionArgs()
            {
                ConfigTypes = new[] { ConfigType.Media }
            });
        }

        private void OnConfigUpdated(object sender, ServerEventArgs e)
        {
            switch (e.Config.Type)
            {
                case ConfigType.Media:
                    ApplyConfig(e.Config as MediaConfig);
                    break;
            }
        }

        private void ApplyConfig(MediaConfig config)
        {
            SetTicker(config.Ticker);
            SetSpeed(config.TickerSpeed);
            Start();
        }

        private void Unloaded()
        {
            Stop();
        }

        public void SetSpeed(int speed)
        {
            this.speed = MillisecondsPerUnit / speed;
        }

        public void SetTicker(string ticker)
        {
            newTicker = ticker;
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void Start()
        {
            if (isRunning)
            {
                return;
            }

            AnimateMove();
            isRunning = true;
        }

        private void AnimateMove()
        {
            if (!String.IsNullOrWhiteSpace(newTicker))
            {
                Ticker = newTicker;
                newTicker = string.Empty;
            }

            control.TickerItem.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
            control.TickerItem.Arrange(new Rect(control.TickerItem.DesiredSize));

            var from = (control.Parent as FrameworkElement).ActualWidth;
            var to = -control.TickerItem.ActualWidth;
            var animationSpeed = TimeSpan.FromMilliseconds((from - to) * speed);

            var ani = new DoubleAnimation()
            {
                From = from,
                To = to,
                Duration = new Duration(animationSpeed)
            };

            Storyboard.SetTarget(ani, control.TickerItem);
            Timeline.SetDesiredFrameRate(ani, 180);

            ani.Completed += AnimationCompleted;

            translateTransform.BeginAnimation(TranslateTransform.XProperty, ani);
        }

        private void AnimationCompleted(object sender, EventArgs e)
        {
            if (isRunning)
            {
                AnimateMove();
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
            if (disposed)
            {
                return;
            }

            try
            {
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
            }
            catch { }

            disposed = true;
        }

        ~TickerControlViewModel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}