using Queue.Hub.Settings;
using Queue.Services.Hub;
using System;

namespace Queue.Hub.Svetovod
{
    public class SvetovodQualityPanelDriver : IHubQualityDriver, IDisposable
    {
        #region events

        public event EventHandler<IHubQualityDriverArgs> Accepted = delegate { };

        #endregion events

        #region fields

        private SvetovodQualityPanelConnection activeConnection;
        private SvetovodQualityPanelDriverConfig config;

        #endregion fields

        public SvetovodQualityPanelDriver(SvetovodQualityPanelDriverConfig config)
        {
            this.config = config;
        }

        public void Enable(byte deviceId)
        {
            CloseActiveConnection();

            if (config.DeviceId == 0 || config.DeviceId == deviceId)
            {
                activeConnection = new SvetovodQualityPanelConnection(config.Port, deviceId);
                activeConnection.Accepted += activeConnection_Accepted;
                activeConnection.Enable();
            }
        }

        public void Disable(byte deviceId)
        {
            CloseActiveConnection();
        }

        private void activeConnection_Accepted(object sender, byte ratting)
        {
            Accepted(this, new HubQualityDriverArgs() { Rating = ratting });
        }

        public void Dispose()
        {
            CloseActiveConnection();
        }

        private void CloseActiveConnection()
        {
            if (activeConnection == null)
            {
                return;
            }

            activeConnection.Accepted -= activeConnection_Accepted;
            activeConnection.Disable();
            activeConnection.Dispose();
            activeConnection = null;
        }
    }
}