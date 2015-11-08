using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts.Server;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Queue.UI.WPF
{
    public class AutoRecoverQueuePlanChannel : IDisposable
    {
        private const int PingInterval = 10000;

        private bool disposed;

        private Channel<IQueuePlanTcpService> Channel;
        private readonly QueuePlanCallback callback;

        private Timer timer;
        private Action<IQueuePlanTcpService> subscribeFunc;

        [Dependency]
        public DuplexChannelManager<IQueuePlanTcpService> ChannelManager { get; set; }

        public AutoRecoverQueuePlanChannel(QueuePlanCallback callback, Action<IQueuePlanTcpService> subscribeFunc)
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

            this.callback = callback;
            this.subscribeFunc = subscribeFunc;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += PingElapsed;
            timer.Start();
        }

        private async void PingElapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            if (timer.Interval < PingInterval)
            {
                timer.Interval = PingInterval;
            }

            if (Channel == null)
            {
                CreateChannel();
            }
            else
            {
                await Ping();
            }

            timer.Start();
        }

        private async Task Ping()
        {
            try
            {
                await Channel.Service.Heartbeat();
            }
            catch
            {
                Reconnect();
            }
        }

        private void Reconnect()
        {
            CloseChannel();
            CreateChannel();
        }

        private void CreateChannel()
        {
            try
            {
                var c = ChannelManager.CreateChannel(callback);
                subscribeFunc(c.Service);

                Channel = c;
            }
            catch { }
        }

        private void CloseChannel()
        {
            if (Channel == null)
            {
                return;
            }

            try
            {
                Channel.Close();
                Channel.Dispose();
                Channel = null;
            }
            catch { }
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
                    if (timer != null)
                    {
                        timer.Stop();
                        timer.Elapsed -= PingElapsed;
                        timer = null;
                    }

                    CloseChannel();
                }
            }
            catch { }
            disposed = true;
        }

        ~AutoRecoverQueuePlanChannel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}