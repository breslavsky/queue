using System;

namespace Queue.UI.WinForms
{
    [Flags]
    public enum QueueMonitorControlOptions
    {
        None = 0,
        OperatorLogin = 1,
        ClientRequestEdit = 2
    }
}