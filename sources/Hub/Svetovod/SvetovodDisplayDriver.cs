using Queue.Services.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayDriver : IHubDisplayDriver, IDisposable
    {
        #region fields

        private SvetovodDisplayConnection activeConnection;
        private SvetovodDisplayDriverConfig config;

        #endregion fields

        public SvetovodDisplayDriver(SvetovodDisplayDriverConfig config)
        {
            this.config = config;
        }

        public void ShowNumber(byte deviceId, short number)
        {
            CloseActiveConnection();

            if (config.DeviceId == 0 || config.DeviceId == deviceId)
            {
                activeConnection = new SvetovodDisplayConnection(config.Port);
                activeConnection.ShowNumber(deviceId, number);
                CloseActiveConnection();
            }
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

            activeConnection.Dispose();
            activeConnection = null;
        }
    }
}