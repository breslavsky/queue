using Queue.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Queue.Hub.Svetovod
{
    public class SvetovodSegmentDisplayConnection : SvetovodConnection, ISvetovodDisplayConnection
    {
        private readonly SvetovodDisplayConnectionConfig config;

        public SvetovodSegmentDisplayConnection(string port, SvetovodDisplayConnectionConfig config) :
            base(port)
        {
            this.config = config;
        }

        public void ShowText(byte sysnum, string number)
        {
            var body = CreateBody(GetBlockContent(number, config.Width));
            WriteToPort(CreateHeader(sysnum, 0x00, 0x00, (byte)(body.Length - 1)), body);
        }

        public void ShowLines(byte sysnum, ushort[][] lines)
        {
            var columnsConfig = config.Columns.Cast<SvetovodDisplayConnectionColumnConfig>().ToArray();
            var content = new List<byte>();

            foreach (var line in lines.Take(config.Width / columnsConfig.Sum(i => i.Width)))
            {
                for (int i = 0; i < line.Length; i++)
                {
                    // передали больше колонок
                    if (i >= columnsConfig.Length)
                    {
                        break;
                    }

                    content.AddRange(GetBlockContent(line[i].ToString(), columnsConfig[i].Width));
                }

                //передали меньше колонок - забьем пустотой
                if (line.Length < columnsConfig.Length)
                {
                    content.AddRange(new byte[columnsConfig.Skip(line.Length).Sum(i => i.Width)]);
                }
            }

            //дозабиваем нулями
            if (content.Count < config.Width)
            {
                content.AddRange(new byte[config.Width - content.Count]);
            }

            var body = CreateBody(content.ToArray());
            WriteToPort(CreateHeader(sysnum, 0x00, 0x00, (byte)(body.Length - 1)), body);
        }

        public void Clear(byte sysnum)
        {
            ShowText(sysnum, "");
        }

        #region protocol

        private static byte[] GetBlockContent(string number, byte width)
        {
            int length = number.Length;

            if (length > width)
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
            for (byte i = 0; i < width - length; i++)
            {
                data.Add(0);
            }

            foreach (var s in units)
            {
                data.Add(GetDigit(s));
            }

            return data.ToArray();
        }

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

        #endregion protocol
    }
}