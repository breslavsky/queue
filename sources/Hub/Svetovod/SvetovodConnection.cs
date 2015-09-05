using NLog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Queue.Hub.Svetovod
{
    public abstract class SvetovodConnection : IDisposable
    {
        private const byte StartByte = 0xe0;
        private const byte SenderByte = 0x00;

        protected readonly Logger logger = LogManager.GetCurrentClassLogger();

        private bool disposed;
        private SerialPort port;

        protected readonly AutoResetEvent receivedResetEvent = new AutoResetEvent(false);
        protected readonly List<byte> receivedBytes = new List<byte>();

        public SvetovodConnection(string port)
        {
            this.port = new SerialPort()
            {
                PortName = port,
                BaudRate = 115200,
                DataBits = 8,
                Parity = Parity.None,
                StopBits = StopBits.One
            };

            this.port.DataReceived += ComPortDataReceived;
            this.port.ErrorReceived += ComPort_ErrorReceived;
            this.port.Open();
        }

        protected void WriteToPort(byte[] bytes)
        {
            port.Write(bytes, 0, bytes.Length);
        }

        protected static byte[] CreateHeader(byte sysnum, byte com1, byte com2, byte com3)
        {
            var header = new List<byte>();
            header.Add(StartByte);
            header.Add(SenderByte);
            header.Add(sysnum);
            header.Add(com1);
            header.Add(com2);
            header.Add(com3);

            var crc = header.Sum(i => i);
            var crcl = Low(crc);
            var crch = (byte)(High(crc) + Low(crc));
            header.Add(crch);
            header.Add(crcl);

            return header.ToArray();
        }

        private static byte Low(int num)
        {
            return (byte)(num & 0xff);
        }

        private static byte High(int num)
        {
            return (byte)(num >> 8);
        }

        private void ComPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            logger.Error(e.EventType);
        }

        private void ComPortDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(200);

            if (!port.IsOpen)
            {
                return;
            }

            receivedBytes.Clear();
            while (port.BytesToRead > 0)
            {
                receivedBytes.Add((byte)port.ReadByte());
            }

            CleanupReceivedData(receivedBytes);
            receivedResetEvent.Set();
        }

        private static void CleanupReceivedData(List<byte> data)
        {
            while (true)
            {
                if (data.Count > 0 && data[0] == 0x00)
                {
                    data.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
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

            if (disposing)
            {
                if (port != null)
                {
                    port.Dispose();
                    port = null;
                }
            }

            disposed = true;
        }

        ~SvetovodConnection()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}