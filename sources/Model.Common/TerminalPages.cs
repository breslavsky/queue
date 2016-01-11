using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum TerminalPages : long
    {
        RequestDate = 1,
        RequestTime = 2
    }
}