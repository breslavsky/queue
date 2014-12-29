using System;

namespace Queue.Common
{
    public struct TicksInterval
    {
        public const long TicksPerSecond = 1000;

        public const long _10Seconds = TicksPerSecond * 10;
        public const long _30Seconds = TicksPerSecond * 30;
        public const long _1Minute = TicksPerSecond * 60;
    }
}