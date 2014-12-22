using System;

namespace Queue.Common
{
    public struct TicksInterval
    {
        public const long _10Seconds = TimeSpan.TicksPerSecond * 10;
        public const long _30Seconds = TimeSpan.TicksPerSecond * 30;
        public const long _1Minute = TimeSpan.TicksPerMinute;
    }
}