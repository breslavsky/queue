using Queue.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Timer = System.Timers.Timer; 

namespace Queue.Hub.Svetovod
{


    public class SvetovodQualityPanelConnection : SvetovodConnection
    {
        private const string NoAnswerErrorMessage = "Не удалось получить ответ от устройства";
        private readonly byte sysnum;

        public event EventHandler<SvetovodQualityPanelConnectionArgs> Accepted = delegate { };

        private Timer stateTimer;

        public SvetovodQualityPanelConnection(string port, byte sysnum) :
            base(port)
        {
            this.sysnum = sysnum;

            stateTimer = new System.Timers.Timer(200);
            stateTimer.Elapsed += stateTimer_Elapsed;
        }

        private void stateTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            stateTimer.Stop();

            try
            {
                receivedResetEvent.Reset();

                WriteToPort(CreateHeader(sysnum, 0x60, 0xff, 0xff));

                if (receivedResetEvent.WaitOne(TimeSpan.FromSeconds(10)))
                {
                    var received = receivedBytes.ToArray();
                    if (received.Length == 10)
                    {
                        logger.Debug("Accepted: [{0}] from [{1}]", received[9], sysnum);
                        Accepted(this, new SvetovodQualityPanelConnectionArgs() { DeviceId = sysnum, Rating = received[9] });
                    }
                }
                else
                {
                    throw new QueueException(NoAnswerErrorMessage);
                }
            }
            catch { }

            if (stateTimer != null)
            {
                stateTimer.Start();
            }
        }

        public void Enable()
        {
            logger.Debug("Enabling [{0}]...", sysnum);

            SetEnabled(true);
            stateTimer.Start();

            logger.Debug("Enabled [{0}]", sysnum);
        }

        public void Disable()
        {
            logger.Debug("Disabling [{0}]....", sysnum);

            stateTimer.Stop();
            SetEnabled(false);

            logger.Debug("Disabled [{0}]", sysnum);
        }

        private void SetEnabled(bool value)
        {
            var header = CreateHeader(sysnum, 0, 0, 2);

            var state = (byte)(value ? 10 : 0);
            var data = new List<byte>();
            data.Add(state);
            data.Add(state);

            var data_src = (byte)data.Sum(i => i);
            data.Add(data_src);

            WriteToPort(header.Concat(data).ToArray());

            if (receivedResetEvent.WaitOne(TimeSpan.FromSeconds(10)))
            {
                var received = receivedBytes.ToArray();
                if (value && (received.Length != 8 || received[3] != 0x00))
                {
                    throw new QueueException("Не удалось активировать устройство");
                }
            }
            else
            {
                throw new QueueException(NoAnswerErrorMessage);
            }
        }

        protected override void OnDispose()
        {
            if (stateTimer != null)
            {
                stateTimer.Elapsed -= stateTimer_Elapsed;
                stateTimer.Stop();
                stateTimer.Dispose();
                stateTimer = null;
            }
        }
    }

    public class SvetovodQualityPanelConnectionArgs
    {
        public byte DeviceId { get; set; }

        public byte Rating { get; set; }
    }
}