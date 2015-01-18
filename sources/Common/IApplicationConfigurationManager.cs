﻿using System;
using System.Configuration;

namespace Queue.Common
{
    public interface IApplicationConfigurationManager
    {
        T GetSection<T>(string key, Action<T> initialize = null) where T : ConfigurationSection, new();

        void Save();
    }
}