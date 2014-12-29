using System;
using System.Windows.Media;
using System.Windows.Threading;

namespace Queue.Notification
{
    public class CustomAnimationTimer
    {
        private int Interval = 1;

        private DispatcherTimer timer;
        private double from;
        private double to;
        private TimeSpan duration;

        private TranslateTransform transform;

        private double delta;
        private int count;

        public CustomAnimationTimer(double from, double to, TimeSpan duration, TranslateTransform transform)
        {
            this.from = from;
            this.to = to;
            this.duration = duration;

            this.transform = transform;

            count = (int)duration.TotalMilliseconds / Interval;
            delta = (to - from) / count;

            timer = new DispatcherTimer();

            timer.Interval = TimeSpan.FromMilliseconds(Interval);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            count--;
            transform.X += delta;
            if (count < 0)
            {
                timer.Stop();
            }
        }
    }
}