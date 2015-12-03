using Microsoft.Practices.Unity;
using NLog;
using Queue.Services.Common;
using Queue.Services.Contracts;
using Queue.Services.Contracts.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;

namespace Queue.Services.Hub
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
                    ConcurrencyMode = ConcurrencyMode.Multiple,
                    IncludeExceptionDetailInFaults = true)]
    public sealed class DisplayHttpService : DisplayService, IDisplayHttpService
    {
        public async void ShowLines(byte deviceId, string lines)
        {
            var data = new List<ushort[]>();

            var rows = lines.Split('|');
            foreach (var r in rows)
            {
                data.Add(r.Split(',').Select(ushort.Parse).ToArray());
            }

            await base.ShowLines(deviceId, data.ToArray());
        }
    }
}