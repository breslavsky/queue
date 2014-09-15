using System;

namespace Queue.Services.Server
{
    public class TimeInterval
    {
        public TimeInterval(TimeSpan startTime, TimeSpan finishTime)
        {
            StartTime = startTime;
            FinishTime = finishTime;
        }

        public TimeSpan StartTime { get; private set; }

        public TimeSpan FinishTime { get; private set; }

        public TimeSpan Duration
        {
            get
            {
                return FinishTime.Subtract(StartTime).Duration();
            }
        }
    }
}