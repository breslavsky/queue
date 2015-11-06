﻿using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Queue.Services.Server
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class TemplateTcpService : TemplateService, ITemplateTcpService
    {
        public async Task<string> GetTemplate(string app, string theme, string template)
        {
            return await Task.Run(() =>
            {
                using (var reader = new StreamReader(base.ReadTemplate(app, theme, template)))
                {
                    return reader.ReadToEnd();
                }
            });
        }
    }
}