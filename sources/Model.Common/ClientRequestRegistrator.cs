﻿using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ClientRequestRegistrator : long
    {
        Terminal = 1,
        Manager = 2,
        Portal = 4
    }
}