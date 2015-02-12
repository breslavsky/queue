using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Queue.UI.WinForms
{
    public partial class DigitalTimer : RichUserControl
    {
        private Timer timer;
        private TimeSpan initTime;

        public DigitalTimer()
        {
            InitializeComponent();

            timer = new Timer();
            timer.Interval = 1000;
            timer.Tick += timer_Tick;
        }

        private void DigitalTimer_Load(object sender, EventArgs e)
        {
            Reset();

            timer.Start();
        }

        public void Reset()
        {
            initTime = DateTime.Now.TimeOfDay;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            timerTextBlock.Text = (DateTime.Now.TimeOfDay - initTime).ToString(@"hh\:mm\:ss");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
                if (timer != null)
                {
                    timer.Dispose();
                }
            }
            base.Dispose(disposing);
        }
    }
}