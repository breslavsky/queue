using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ClientRequestRegistrator
    {
        None = 0,
        Terminal = 1,
        Manager = 2,
        Portal = 4
    }
}