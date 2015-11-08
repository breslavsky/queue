using Queue.Services.Contracts.Server;
using Queue.Services.DTO;
using Queue.UI.WPF;
using System;

namespace Queue.Notification
{
    public class ClientRequestsListener : IDisposable
    {
        private bool disposed;

        private AutoRecoverQueuePlanChannel channel;

        public event EventHandler<ClientRequest> ClientRequestUpdated = delegate { };

        public event EventHandler<ClientRequest> CallClient = delegate { };

        public ClientRequestsListener()
        {
            channel = new AutoRecoverQueuePlanChannel(CreateQueuePlanCallback(), Subscribe);
        }

        private QueuePlanCallback CreateQueuePlanCallback()
        {
            var result = new QueuePlanCallback();
            result.OnClientRequestUpdated += OnClientRequestUpdated;
            result.OnCallClient += OnCallClient;

            return result;
        }

        private void OnCallClient(object sender, QueuePlanEventArgs e)
        {
            ClientRequestUpdated(this, e.ClientRequest);
            CallClient(this, e.ClientRequest);
        }

        private void OnClientRequestUpdated(object sender, QueuePlanEventArgs e)
        {
            ClientRequestUpdated(this, e.ClientRequest);
        }

        private void Subscribe(IQueuePlanTcpService service)
        {
            service.Subscribe(QueuePlanEventType.ClientRequestUpdated);
            service.Subscribe(QueuePlanEventType.CallClient);
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

        ~ClientRequestsListener()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}