﻿using System;

namespace Queue.Model.Common
{
    [Flags]
    public enum ServiceRenderingMode
    {
        LiveRequests = 1,
        EarlyRequests = 2,
        AllRequests = LiveRequests | EarlyRequests
    }

    public static partial class TranslationExtensions
    {
        public static string Translate(this ServiceRenderingMode value)
        {
            return Translation.ServiceRenderingMode.ResourceManager.GetString(value.ToString());
        }
    }
}