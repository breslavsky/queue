using Queue.Common;
using Queue.Services.Hub;
using System;
using System.Linq;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayDriver : IHubDisplayDriver, IDisposable
    {
        #region fields

        private bool disposed;
        private ISvetovodDisplayConnection activeConnection;
        private SvetovodDisplayDriverConfig config;

        #endregion fields

        public SvetovodDisplayDriver(SvetovodDisplayDriverConfig config)
        {
            this.config = config;
        }

        public void ShowText(byte deviceId, string text)
        {
            CloseActiveConnection();

            if (config.DeviceId != 0 && config.DeviceId != deviceId)
            {
                return;
            }

            var conf = GetDeviceConfig(deviceId);

            switch (conf.Type)
            {
                case SvetovodDisplayType.Segment:
                    ShowTextOnSegmentDisplay(deviceId, text);
                    break;

                case SvetovodDisplayType.Matrix:
                    ShowTextOnMatrixDisplay(deviceId, text, conf);
                    break;
            }

            CloseActiveConnection();
        }

        public void ClearText(byte deviceId)
        {
            CloseActiveConnection();

            if (config.DeviceId != 0 && config.DeviceId != deviceId)
            {
                return;
            }

            var conf = GetDeviceConfig(deviceId);

            switch (conf.Type)
            {
                case SvetovodDisplayType.Segment:
                    ClearTextOnSegmentDisplay(deviceId);
                    break;

                case SvetovodDisplayType.Matrix:
                    ClearTextOnMatrixDisplay(deviceId, conf);
                    break;
            }

            CloseActiveConnection();
        }

        private SvetovodDisplayConnectionConfig GetDeviceConfig(byte deviceId)
        {
            var connections = config.Connections.Cast<SvetovodDisplayConnectionConfig>();
            var conf = connections.Where(c => c.Sysnum == deviceId)
                                    .FirstOrDefault();

            if (conf == null)
            {
                conf = connections.Where(c => c.Sysnum == 0)
                                        .FirstOrDefault();
            }

            if (conf == null)
            {
                throw new QueueException("Не найдена конфигурация для устройства [id: {0}]", deviceId);
            }

            return conf;
        }

        private void ShowTextOnSegmentDisplay(byte sysnum, string text)
        {
            var connection = new SvetovodSegmentDisplayConnection(config.Port);
            short number;
            if (!Int16.TryParse(text, out number))
            {
                throw new QueueException("Данное табло [id: {0}] поддерживает вывод только цифровой информации", sysnum);
            }

            connection.ShowNumber(sysnum, number);

            activeConnection = connection;
        }

        private void ClearTextOnSegmentDisplay(byte sysnum)
        {
            var connection = new SvetovodSegmentDisplayConnection(config.Port);
            connection.ClearNumber(sysnum);

            activeConnection = connection;
        }

        private void ShowTextOnMatrixDisplay(byte sysnum, string text, SvetovodDisplayConnectionConfig conf)
        {
            var connection = new SvetovodMatrixDisplayConnection(config.Port);
            connection.ShowText(sysnum, text, conf.Width, conf.Height);

            activeConnection = connection;
        }

        private void ClearTextOnMatrixDisplay(byte sysnum, SvetovodDisplayConnectionConfig conf)
        {
            var connection = new SvetovodMatrixDisplayConnection(config.Port);
            connection.ShowText(sysnum, " ", conf.Width, conf.Height);

            activeConnection = connection;
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

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                OnDispose();

                CloseActiveConnection();
            }

            disposed = true;
        }

        protected virtual void OnDispose()
        {
        }

        ~SvetovodDisplayDriver()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}