using System;

namespace Queue.Administrator
{
    [Flags]
    public enum QueueMonitorControlOptions
    {
        None = 0,
        OperatorLogin = 1,
        ClientRequestEdit = 2
    }
}