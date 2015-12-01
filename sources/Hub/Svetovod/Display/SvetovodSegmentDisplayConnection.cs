using Queue.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Queue.Hub.Svetovod
{
    public class SvetovodSegmentDisplayConnection : SvetovodConnection, ISvetovodDisplayConnection
    {
        public SvetovodSegmentDisplayConnection(string port) :
            base(port)
        {
        }

        public void ShowNumber(byte sysnum, string number, byte width)
        {
            var body = CreateBody(GetBodyContent(sysnum, number, width));
            WriteToPort(CreateHeader(sysnum, 0x00, 0x00, (byte)(body.Length - 1)), body);
        }

        #region protocol

        private static byte GetDigit(byte digit)
        {
            var bits = new[] { true, true, true, true, true, true, false, false };

            switch (digit)
            {
                case 1:
                    bits = new[] { false, true, true, false, false, false, false, false };
                    break;

                case 2:
                    bits = new[] { true, true, false, true, true, false, true, false };
                    break;

                case 3:
                    bits = new[] { true, true, true, true, false, false, true, false };
                    break;

                case 4:
                    bits = new[] { false, true, true, false, false, true, true, false };
                    break;

                case 5:
                    bits = new[] { true, false, true, true, false, true, true, false };
                    break;

                case 6:
                    bits = new[] { true, false, true, true, true, true, true, false };
                    break;

                case 7:
                    bits = new[] { true, true, true, false, false, false, false, false };
                    break;

                case 8:
                    bits = new[] { true, true, true, true, true, true, true, false };
                    break;

                case 9:
                    bits = new[] { true, true, true, true, false, true, true, false };
                    break;
            }

            var bytes = new byte[] { 0 };
            new BitArray(bits).CopyTo(bytes, 0);
            return bytes.First();
        }

        private static byte[] GetBodyContent(byte sysnum, string number, byte segments)
        {
            int lenght = number.Length;

            if (lenght > segments)
            {
                throw new QueueException();
            }

            var digits = number.ToCharArray();

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

            return data.ToArray();
        }

        #endregion protocol

        public void ClearNumber(byte sysnum)
        {
            //TODO
        }
    }
}