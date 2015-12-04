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

            var connection = GetConnectionForDevice(deviceId);
            if (connection == null)
            {
                return;
            }

            connection.ShowText(deviceId, text);

            activeConnection = connection;
            CloseActiveConnection();
        }

        public void ShowLines(byte deviceId, ushort[][] lines)
        {
            logger.Debug("show lines [device: {0}; lines: {1}]", deviceId, lines.Length);

            CloseActiveConnection();

            var connection = GetConnectionForDevice(deviceId);
            if (connection == null)
            {
                return;
            }

            connection.ShowLines(deviceId, lines);

            activeConnection = connection;
            CloseActiveConnection();
        }

        public void ClearText(byte deviceId)
        {
            logger.Debug("clear text [device: {0}]", deviceId);

            CloseActiveConnection();

            var connection = GetConnectionForDevice(deviceId);
            if (connection == null)
            {
                return;
            }

            connection.Clear(deviceId);

            activeConnection = connection;
            CloseActiveConnection();
        }

        private ISvetovodDisplayConnection GetConnectionForDevice(byte deviceId)
        {
            if (config.DeviceId != 0 && config.DeviceId != deviceId)
            {
                return null;
            }

            var conf = GetDeviceConfig(deviceId);

            switch (conf.Type)
            {
                case SvetovodDisplayType.Segment:
                    return new SvetovodSegmentDisplayConnection(config.Port, conf);

                case SvetovodDisplayType.Matrix:
                    return new SvetovodMatrixDisplayConnection(config.Port, conf);

                default:
                    throw new QueueException("Данный вид табло не поддерживается: {0}", conf.Type);
            }
        }

        private SvetovodDisplayConnectionConfig GetDeviceConfig(byte deviceId)
        {
            var connections = config.Connections.Cast<SvetovodDisplayConnectionConfig>();
            var conf = connections.Where(c => c.Sysnum == deviceId)
                                    .FirstOrDefault();

            if (conf == null)
            {
                conf = connections.Where(c => c.Sysnum == 0).FirstOrDefault();
            }

            if (conf == null)
            {
                throw new QueueException("Не найдена конфигурация для устройства [id: {0}]", deviceId);
            }

            return conf;
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