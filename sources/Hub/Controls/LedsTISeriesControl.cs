using Junte.UI.WinForms;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;

namespace Queue.Hub
{
    public partial class LedsTISeriesControl : UserControl
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LedsTISeriesControl));

        private static Properties.Settings settings = Properties.Settings.Default;

        private SerialPort port;

        public LedsTISeriesControl()
        {
            InitializeComponent();

            if (settings.LedsTISeries == null)
            {
                settings.LedsTISeries = new LedsTISeriesSettings();
            }
        }

        private Controller controller
        {
            get
            {
                return ControllerManager.Current;
            }
        }

        private SerialPort Port
        {
            get
            {
                if (port != null && !port.IsOpen)
                {
                    port.Open();
                }

                return port;
            }
        }

        private string PortName
        {
            get
            {
                return portComboBox.SelectedItem != null ? portComboBox.SelectedItem.ToString() : null;
            }
            set
            {
                portComboBox.SelectedItem = value;
            }
        }

        private void LedsTISeriesSerialController_Load(object sender, EventArgs e)
        {
            logger.Info("Инициализация");

            portComboBox.Items.Add(string.Empty);
            foreach (var n in SerialPort.GetPortNames())
            {
                portComboBox.Items.Add(n);
            }

            if (!string.IsNullOrWhiteSpace(settings.LedsTISeries.PortName))
            {
                PortName = settings.LedsTISeries.PortName;
            }

            controller.OnCallingClientRequestsChanged += controller_OnCallingClientRequestsChanged;
        }

        private void controller_OnCallingClientRequestsChanged(object sender, ControllerEventArgs e)
        {
            Invoke((MethodInvoker)delegate
            {
                refresh();
            });
        }

        private void portComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Port != null)
            {
                try
                {
                    Port.Close();
                }
                catch (Exception exception)
                {
                    logger.Warn(exception);
                }
            }

            settings.LedsTISeries.PortName = PortName;
            settings.Save();

            if (!string.IsNullOrWhiteSpace(PortName))
            {
                port = new SerialPort(PortName, 115200, Parity.None, 8, StopBits.One);
                try
                {
                    port.Open();
                }
                catch (Exception exception)
                {
                    UIHelper.Warning(exception.Message);
                    return;
                }

                logger.InfoFormat("Соединение с [{0}] установлено", PortName);

                refresh();
            }
        }

        private void refresh()
        {
            if (Port != null)
            {
                foreach (var w in controller.Workplaces)
                {
                    var clientRequest = controller.CallingClientRequests.FirstOrDefault(r => w.Equals(r.Operator.Workplace));
                    logger.Debug(w);
                    display(w.Display, (short)(clientRequest != null ? clientRequest.Number : -1), w.Segments);
                }
            }
        }

        private void display(byte address, short number, byte segments)
        {
            byte command = 0x00;

            byte[] body = getBody(address, number, segments);

            List<byte> buffer = new List<byte>();

            buffer.AddRange(getHeader(address, command, body.Length - 1));
            buffer.AddRange(body);

            byte[] data = buffer.ToArray();
            logger.DebugFormat("Запись в [{0}]: [{1}]", PortName, string.Join(" ", data));
            Port.Write(data, 0, data.Length);
        }

        #region protocol

        private const byte MAX_SEGMENTS = 3;

        private static byte hight(int number)
        {
            return Convert.ToByte(number >> 8);
        }

        private static byte low(int number)
        {
            return Convert.ToByte(number & 0x00FF);
        }

        private static byte[] getHeader(byte address, byte command, int lenght)
        {
            byte sender = 0;

            byte com0 = command;
            byte com1 = 0;
            byte com2 = (byte)lenght;

            List<byte> data = new List<byte>()
            {
                0xE0,
                sender,
                address,
                com0,
                com1,
                com2
            };

            int crc = (int)data.Sum(x => x);

            byte crcl = low(crc);
            byte crch = (byte)(hight(crc) + crcl);

            data.Add(crch);
            data.Add(crcl);

            return data.ToArray();
        }

        private static byte getDigit(byte digit)
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

        private static byte[] getBody(byte address, short number, byte segments)
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
                data.Add(getDigit(s));
            }

            int crc = (int)data.Sum(x => x);
            data.Add((byte)crc);

            return data.ToArray();
        }

        #endregion protocol
    }
}