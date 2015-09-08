using Queue.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Queue.Hub.Svetovod
{
    public class SvetovodQualityPanelConnection : SvetovodConnection
    {
        private readonly byte sysnum;

        public event EventHandler<byte> Accepted = delegate { };

        private System.Timers.Timer stateTimer;

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
                        Accepted(this, received[9]);
                    }
                }
            }
            catch { }

            stateTimer.Start();
        }

        public void Enable()
        {
            SetEnabled(true);
            stateTimer.Start();
        }

        public void Disable()
        {
            stateTimer.Stop();
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

            WriteToPort(header.Concat(data).ToArray());

            if (receivedResetEvent.WaitOne(TimeSpan.FromSeconds(10)))
            {
                var received = receivedBytes.ToArray();
                if (received.Length != 8 || received[3] != 0x00)
                {
                    throw new QueueException("Не удалось активировать устройство");
                }
            }
        }

        protected override void OnDispose()
        {
            if (stateTimer != null)
            {
                stateTimer.Stop();
                stateTimer.Dispose();
                stateTimer = null;
            }
        }
    }
}