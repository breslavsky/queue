using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ServiceRenderingMode : long
    {
        LiveRequests = 1,
        EarlyRequests = 2,
        AllRequests = LiveRequests | EarlyRequests
    }
}