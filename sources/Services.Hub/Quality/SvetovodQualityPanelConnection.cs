using Queue.Common;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Queue.Services.Hub.Quality
{
    public class SvetovodQualityPanelConnection : IDisposable
    {
        private const byte StartByte = 0xe0;
        private const byte SenderByte = 0x00;

        private bool disposed;

        private readonly SerialPort port;
        private readonly byte sysnum;
        private readonly AutoResetEvent receivedResetEvent = new AutoResetEvent(false);
        private readonly List<byte> receivedBytes = new List<byte>();

        public event EventHandler<HubQualityDriverArgs> Accepted = delegate { };

        public SvetovodQualityPanelConnection(string port, byte sysnum)
        {
            this.sysnum = sysnum;
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

        public void Enable()
        {
            SetEnabled(true);
            Listen();
        }

        public void Disable()
        {
            SetEnabled(false);
        }

        private void SetEnabled(bool value)
        {
            var header = CreateHeader(2, 0, 0, 2);

            var state = (byte)(value ? 10 : 0);
            var data = new List<byte>();
            data.Add(state);
            data.Add(state);

            var data_src = (byte)data.Sum(i => i);
            data.Add(data_src);

            var bytes = header.Concat(data).ToArray();
            port.Write(bytes, 0, bytes.Length);

            if (receivedResetEvent.WaitOne(TimeSpan.FromSeconds(10)))
            {
                var received = receivedBytes.ToArray();
                if (received.Length != 8 || received[3] != 0x00)
                {
                    throw new QueueException("Не удалось активировать устройство");
                }
            }
        }

        private void Listen()
        {
            while (true)
            {
                receivedResetEvent.Reset();

                var header = CreateHeader(2, 0x60, 0xff, 0xff);
                port.Write(header, 0, header.Length);

                if (receivedResetEvent.WaitOne(TimeSpan.FromSeconds(10)))
                {
                    var received = receivedBytes.ToArray();
                    if (received.Length == 10)
                    {
                        Accepted(this, new HubQualityDriverArgs()
                            {
                                Rating = received[9]
                            });

                        break;
                    }
                }

                Thread.Sleep(200);
            }
        }

        private static byte[] CreateHeader(byte sysnum, byte com1, byte com2, byte com3)
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
                }
            }

            disposed = true;
        }

        ~SvetovodQualityPanelConnection()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}