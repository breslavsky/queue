using Junte.WCF;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;

namespace Queue.Services.Media
{
    public class MediaServiceHost : ServiceHost
    {
        public MediaServiceHost(params Uri[] baseAddresses)
            : base(typeof(MediaService), baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new MediaServiceProvider());
            }
        }
    }
}