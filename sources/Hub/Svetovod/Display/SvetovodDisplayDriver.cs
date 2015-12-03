using NLog;
using Queue.Common;
using Queue.Services.Hub;
using System;
using System.Linq;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayDriver : IDisplayDriver, IDisposable
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

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
            logger.Debug("show text [device: {0}; text: {1}]", deviceId, text);

            CloseActiveConnection();

            if (config.DeviceId != 0 && config.DeviceId != deviceId)
            {
                return;
            }

            var conf = GetDeviceConfig(deviceId);

            switch (conf.Type)
            {
                case SvetovodDisplayType.Segment:
                    ShowTextOnSegmentDisplay(deviceId, text, conf);
                    break;

                case SvetovodDisplayType.Matrix:
                    ShowTextOnMatrixDisplay(deviceId, text, conf);
                    break;
            }

            CloseActiveConnection();
        }

        public void ShowLines(byte deviceId, ushort[][] lines)
        {
            logger.Debug("show lines [device: {0}; lines: {1}]", deviceId, lines.Length);

            CloseActiveConnection();

            if (config.DeviceId != 0 && config.DeviceId != deviceId)
            {
                return;
            }

            var conf = GetDeviceConfig(deviceId);

            switch (conf.Type)
            {
                case SvetovodDisplayType.Segment:
                    throw new NotImplementedException();
                    break;

                case SvetovodDisplayType.Matrix:
                    throw new NotImplementedException();
                    break;
            }

            CloseActiveConnection();
        }

        public void ClearText(byte deviceId)
        {
            logger.Debug("clear text [device: {0}]", deviceId);

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

        private void ShowTextOnSegmentDisplay(byte sysnum, string text, SvetovodDisplayConnectionConfig conf)
        {
            var connection = new SvetovodSegmentDisplayConnection(config.Port);

            connection.ShowNumber(sysnum, text, conf.Width);

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