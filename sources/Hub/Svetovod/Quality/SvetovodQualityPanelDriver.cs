using NLog;
using Queue.Hub.Settings;
using Queue.Services.Hub;
using System;
using System.Collections.Generic;

namespace Queue.Hub.Svetovod
{
    public class SvetovodQualityPanelDriver : IQualityDriver, IDisposable
    {
        #region events

        public event EventHandler<IQualityDriverArgs> Accepted = delegate { };

        #endregion events

        #region fields

        private readonly Logger logger = LogManager.GetCurrentClassLogger();
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
                try
                {
                    var connection = new SvetovodQualityPanelConnection(config.Port, deviceId);
                    connection.Enable();
                    connection.Accepted += activeConnection_Accepted;
                    activeConnection = connection;
                }
                catch (Exception e)
                {
                    logger.Error(e);
                    throw;
                }
            }
        }

        public void Disable(byte deviceId)
        {
            CloseActiveConnection();
        }

        private void activeConnection_Accepted(object sender, SvetovodQualityPanelConnectionArgs args)
        {
            Answers[args.DeviceId] = args.Rating;
            Accepted(this, new HubQualityDriverArgs() { DeviceId = args.DeviceId, Rating = args.Rating });
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