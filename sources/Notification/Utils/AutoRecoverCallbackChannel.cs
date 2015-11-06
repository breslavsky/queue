using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Queue.Notification
{
    public class AutoRecoverCallbackChannel : IDisposable
    {
        private const int PingInterval = 10000;

        private bool disposed;

        private Channel<IServerTcpService> channel;
        private readonly QueuePlanCallback callback;

        private Timer timer;
        private Action<IServerTcpService> subscribeFunc;

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        public AutoRecoverCallbackChannel(QueuePlanCallback callback, Action<IServerTcpService> subscribeFunc)
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

            if (channel == null)
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
                await TaskPool.AddTask(channel.Service.GetDateTime());
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

                channel = c;
            }
            catch { }
        }

        private void CloseChannel()
        {
            if (channel == null)
            {
                return;
            }

            try
            {
                channel.Close();
                channel.Dispose();
                channel = null;
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

        ~AutoRecoverCallbackChannel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}