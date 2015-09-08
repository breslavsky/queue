using Queue.Hub.Settings;
using Queue.Services.Hub;
using System;
using System.Collections.Generic;

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

        #region properties

        public Dictionary<byte, byte> Answers { get; set; }

        #endregion properties

        public SvetovodQualityPanelDriver(SvetovodQualityPanelDriverConfig config)
        {
            this.config = config;
            Answers = new Dictionary<byte, byte>();
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
            Answers[config.DeviceId] = ratting;
            Accepted(this, new HubQualityDriverArgs() { DeviceId = config.DeviceId, Rating = ratting });
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