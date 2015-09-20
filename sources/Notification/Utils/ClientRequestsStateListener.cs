using Junte.WCF;
using Microsoft.Practices.Unity;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;

namespace Queue.Notification
{
    public class ClientRequestsStateListener : IDisposable
    {
        private AutoRecoverCallbackChannel channel;

        [Dependency]
        public DuplexChannelManager<IServerTcpService> ChannelManager { get; set; }

        public event EventHandler<ClientRequest> ClientRequestUpdated = delegate { };

        public event EventHandler<ClientRequest> CallClient = delegate { };

        private bool disposed;

        public ClientRequestsStateListener()
        {
            channel = new AutoRecoverCallbackChannel(CreateServerCallback(), Subscribe);
        }

        private ServerCallback CreateServerCallback()
        {
            var result = new ServerCallback();
            result.OnClientRequestUpdated += OnClientRequestUpdated;
            result.OnCallClient += OnCallClient;

            return result;
        }

        private void OnCallClient(object sender, ServerEventArgs e)
        {
            ClientRequestUpdated(this, e.ClientRequest);
            CallClient(this, e.ClientRequest);
        }

        private void OnClientRequestUpdated(object sender, ServerEventArgs e)
        {
            ClientRequestUpdated(this, e.ClientRequest);
        }

        private void Subscribe(IServerTcpService service)
        {
            service.Subscribe(ServerServiceEventType.ClientRequestUpdated);
            service.Subscribe(ServerServiceEventType.CallClient);
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
                }
            }
            catch { }

            disposed = true;
        }

        ~ClientRequestsStateListener()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}