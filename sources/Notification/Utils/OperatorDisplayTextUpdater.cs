using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.Threading.Tasks;

namespace Queue.Notification.Utils
{
    public class OperatorDisplayTextUpdater : IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private object displayLock = new object();
        private bool disposed;

        [Dependency]
        public ClientRequestsListener ClientRequestsListener { get; set; }

        [Dependency]
        public ChannelManager<IHubDisplayTcpService> HubDisplayChannelManager { get; set; }

        [Dependency]
        public HubSettings HubSettings { get; set; }

        public OperatorDisplayTextUpdater()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

            ClientRequestsListener.CallClient += OnCallClient;
            ClientRequestsListener.ClientRequestUpdated += OnClientRequestUpdated;
        }

        private void OnCallClient(object sender, ClientRequest request)
        {
            Task.Run(() =>
            {
                lock (displayLock)
                {
                    ShowDisplayText(request);
                }
            });
        }

        private void OnClientRequestUpdated(object sender, ClientRequest request)
        {
            Task.Run(() =>
            {
                lock (displayLock)
                {
                    ClearDisplayTextIfNeed(request);
                }
            });
        }

        private void ShowDisplayText(ClientRequest request)
        {
            if (!HubSettings.Enabled)
            {
                return;
            }

            try
            {
                using (var channel = HubDisplayChannelManager.CreateChannel())
                {
                    channel.Service.ShowText(request.Operator.Workplace.DisplayDeviceId, request.Number.ToString());
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private void ClearDisplayTextIfNeed(ClientRequest request)
        {
            if (!HubSettings.Enabled || !NeedClearText(request))
            {
                return;
            }

            try
            {
                using (var channel = HubDisplayChannelManager.CreateChannel())
                {
                    channel.Service.ClearText(request.Operator.Workplace.DisplayDeviceId);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private bool NeedClearText(ClientRequest request)
        {
            return request.State == ClientRequestState.Rendered ||
                    request.State == ClientRequestState.Canceled;
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
                    ClientRequestsListener.CallClient -= OnCallClient;
                    ClientRequestsListener.ClientRequestUpdated -= OnClientRequestUpdated;
                }
            }
            catch { }

            disposed = true;
        }

        ~OperatorDisplayTextUpdater()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}