using Junte.Parallel;
using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Common;
using Queue.Services.Contracts;
using System;
using System.Timers;

namespace Queue.Notification
{
    public class AutoRecoverCallbackChannel : IDisposable
    {
        private const int PingInterval = 10000;

        private bool disposed;

        private Channel<IServerTcpService> channel;
        private readonly ServerCallback callback;

        private Timer timer;
        private Action<IServerTcpService> subscribeFunc;

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        [Dependency]
        public TaskPool TaskPool { get; set; }

        public AutoRecoverCallbackChannel(ServerCallback callback, Action<IServerTcpService> subscribeFunc)
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

            this.callback = callback;
            this.subscribeFunc = subscribeFunc;

            timer = new Timer();
            timer.Interval = 1000;
            timer.Elapsed += PingElapsed;

            CreateChannel();
            timer.Start();
        }

        private async void PingElapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();

            if (timer.Interval < PingInterval)
            {
                timer.Interval = PingInterval;
            }

            try
            {
                await TaskPool.AddTask(channel.Service.GetDateTime());
            }
            catch
            {
                Reconnect();
            }

            timer.Start();
        }

        private void Reconnect()
        {
            channel.Dispose();
            CreateChannel();
        }

        private void CreateChannel()
        {
            channel = ChannelManager.CreateChannel(callback);
            subscribeFunc(channel.Service);
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
                if (timer != null)
                {
                    timer.Stop();
                    timer.Elapsed -= PingElapsed;
                    timer = null;
                }

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

        ~AutoRecoverCallbackChannel()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}