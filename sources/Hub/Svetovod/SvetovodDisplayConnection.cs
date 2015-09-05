using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayConnection : SvetovodConnection
    {
        #region fields

        private const byte Segments = 0x04;

        #endregion fields

        public SvetovodDisplayConnection(string port) :
            base(port)
        {
        }

        public void ShowNumber(byte sysnum, short number)
        {
            var body = GetBody(sysnum, number, Segments);
            var buffer = new List<byte>();

            buffer.AddRange(CreateHeader(sysnum, 0x00, 0x00, (byte)(body.Length - 1)));
            buffer.AddRange(body);

            byte[] data = buffer.ToArray();
            logger.Debug("Запись [{0}]", String.Join(" ", data));
            WriteToPort(data);
        }

        #region protocol

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

            var digits = source.ToCharArray();

            var units = new List<byte>();
            foreach (var d in digits)
            {
                units.Add(byte.Parse(d.ToString()));
            }

            var data = new List<byte>();
            for (byte i = 0; i < segments - lenght; i++)
            {
                data.Add(0);
            }

            foreach (var s in units)
            {
                data.Add(GetDigit(s));
            }

            int crc = data.Sum(x => x);
            data.Add((byte)crc);

            return data.ToArray();
        }

        #endregion protocol
    }
}