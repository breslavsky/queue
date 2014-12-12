using System;

namespace Queue.Manager
{
    [Flags]
    public enum QueueMonitorControlOptions
    {
        None = 0,
        OperatorLogin = 1,
        ClientRequestEdit = 2
    }
}