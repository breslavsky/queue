using NLog;
using Queue.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayConnection : IDisposable
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region fields

        private const byte StartByte = 0xe0;
        private const byte SenderByte = 0x00;
        private const byte Segments = 0x03;

        private readonly SerialPort port;
        private bool disposed;

        #endregion fields

        public SvetovodDisplayConnection(string port)
        {
            this.port = new SerialPort()
             {
                 PortName = port,
                 BaudRate = 115200,
                 DataBits = 8,
                 Parity = Parity.None,
                 StopBits = StopBits.One
             };
            this.port.Open();
        }

        public void ShowNumber(byte sysnum, short number)
        {
            byte[] body = GetBody(sysnum, number, Segments);
            List<byte> buffer = new List<byte>();

            buffer.AddRange(CreateHeader(sysnum, 0x00, 0x00, (byte)(body.Length - 1)));
            buffer.AddRange(body);

            byte[] data = buffer.ToArray();
            logger.Debug("Запись [{0}]", string.Join(" ", data));
            port.Write(data, 0, data.Length);
        }

        #region protocol

        private static byte High(int number)
        {
            return Convert.ToByte(number >> 8);
        }

        private static byte Low(int number)
        {
            return Convert.ToByte(number & 0x00FF);
        }

        private static byte[] CreateHeader(byte sysnum, byte com0, byte com1, byte com2)
        {
            var header = new List<byte>();
            header.Add(StartByte);
            header.Add(SenderByte);
            header.Add(sysnum);
            header.Add(com0);
            header.Add(com1);
            header.Add(com2);

            var crc = header.Sum(i => i);
            var crcl = Low(crc);
            var crch = (byte)(High(crc) + Low(crc));
            header.Add(crch);
            header.Add(crcl);

            return header.ToArray();
        }

        private static byte GetDigit(byte digit)
        {
            bool[] bits = new bool[] { true, true, true, true, true, true, false, false };

            switch (digit)
            {
                case 1:
                    bits = new bool[] { false, true, true, false, false, false, false, false };
                    break;

                case 2:
                    bits = new bool[] { true, true, false, true, true, false, true, false };
                    break;

                case 3:
                    bits = new bool[] { true, true, true, true, false, false, true, false };
                    break;

                case 4:
                    bits = new bool[] { false, true, true, false, false, true, true, false };
                    break;

                case 5:
                    bits = new bool[] { true, false, true, true, false, true, true, false };
                    break;

                case 6:
                    bits = new bool[] { true, false, true, true, true, true, true, false };
                    break;

                case 7:
                    bits = new bool[] { true, true, true, false, false, false, false, false };
                    break;

                case 8:
                    bits = new bool[] { true, true, true, true, true, true, true, false };
                    break;

                case 9:
                    bits = new bool[] { true, true, true, true, false, true, true, false };
                    break;
            }

            byte[] bytes = new byte[] { 0 };
            new BitArray(bits).CopyTo(bytes, 0);
            return bytes.First();
        }

        private static byte[] GetBody(byte sysnum, short number, byte segments)
        {
            string source = number.ToString();

            if (number < 0)
            {
                source = string.Empty;
            }

            int lenght = source.Length;

            if (lenght > segments)
            {
                throw new Exception();
            }

            char[] digits = source.ToCharArray();

            List<byte> units = new List<byte>();
            foreach (var d in digits)
            {
                units.Add(byte.Parse(d.ToString()));
            }

            List<byte> data = new List<byte>();
            for (byte i = 0; i < segments - lenght; i++)
            {
                data.Add(0);
            }

            foreach (var s in units)
            {
                data.Add(GetDigit(s));
            }

            int crc = (int)data.Sum(x => x);
            data.Add((byte)crc);

            return data.ToArray();
        }

        #endregion protocol

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

        ~SvetovodDisplayConnection()
        {
            Dispose(false);
        }

        #endregion IDisposable
    }
}