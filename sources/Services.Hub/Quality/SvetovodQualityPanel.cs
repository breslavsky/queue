using System;

namespace Queue.Services.Hub.Quality
{
    public class SvetovodQualityPanel : IHubQualityDriver
    {
        public event EventHandler<HubQualityDriverArgs> Accepted = delegate { };

        private SvetovodQualityPanelConnection activeConnection;

        public void Enable(int deviceId)
        {
            CloseActiveConnection();

            //TODO читать параметры
            activeConnection = new SvetovodQualityPanelConnection("COM5", 2);
            activeConnection.Accepted += activeConnection_Accepted;
            activeConnection.Enable();
        }

        public void Disable(int deviceId)
        {
            CloseActiveConnection();
        }

        private void activeConnection_Accepted(object sender, HubQualityDriverArgs e)
        {
            Accepted(this, e);
        }

        private void CloseActiveConnection()
        {
            if (activeConnection == null)
            {
                return;
            }

            activeConnection.Disable();
            activeConnection.Dispose();
            activeConnection = null;
        }
    }
}