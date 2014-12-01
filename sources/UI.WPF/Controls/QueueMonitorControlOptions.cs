using System;

namespace Queue.UI.WPF
{
    [Flags]
    public enum QueueMonitorControlOptions
    {
        None = 0,
        OperatorLogin = 1,
        ClientRequestEdit = 2
    }
}