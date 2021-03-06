﻿using Junte.WCF;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using NLog;
using Queue.Common.Settings;
using Queue.Model.Common;
using Queue.Services.Contracts.Hub;
using Queue.Services.DTO;
using System;
using System.Threading;

namespace Queue.Notification.Utils
{
    public class OperatorDisplayTextUpdater : IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed;

        [Dependency]
        public ClientRequestsListener ClientRequestsListener { get; set; }

        [Dependency]
        public ChannelManager<IDisplayTcpService> DisplayChannelManager { get; set; }

        [Dependency]
        public HubSettings HubSettings { get; set; }

        public OperatorDisplayTextUpdater()
        {
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(this);

            logger.Debug("endpoint: {0}; enabled: {1}", HubSettings.Endpoint, HubSettings.Enabled);

            ClientRequestsListener.CallClient += OnCallClient;
            ClientRequestsListener.ClientRequestUpdated += OnClientRequestUpdated;
        }

        private void OnCallClient(object sender, ClientRequest request)
        {
            ShowDisplayText(request);
        }

        private void OnClientRequestUpdated(object sender, ClientRequest request)
        {
            ClearDisplayTextIfNeed(request);
        }

        private async void ShowDisplayText(ClientRequest request)
        {
            if (!HubSettings.Enabled)
            {
                return;
            }

            try
            {

                logger.Debug("show text [device: {0}; text: {1}]", request.Operator.Workplace.DisplayDeviceId, request.Number);

                using (var channel = DisplayChannelManager.CreateChannel())
                {
                    await channel.Service.ShowText(request.Operator.Workplace.DisplayDeviceId, request.Number.ToString());
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
            }
        }

        private async void ClearDisplayTextIfNeed(ClientRequest request)
        {
            if (!HubSettings.Enabled || !NeedClearText(request))
            {
                return;
            }

            try
            {
                logger.Debug("clear text [device: {0}]", request.Operator.Workplace.DisplayDeviceId);

                using (var channel = DisplayChannelManager.CreateChannel())
                {
                    await channel.Service.ClearText(request.Operator.Workplace.DisplayDeviceId);
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