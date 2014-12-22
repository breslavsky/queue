using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System;
using System.ServiceModel;

namespace Queue.Services.Media
{
    public class MediaServiceHost : ServiceHost
    {
        public MediaServiceHost(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser,
            string folder, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var d in this.ImplementedContracts.Values)
            {
                d.Behaviors.Add(new MediaServiceProvider(channelBuilder, currentUser, folder));
            }
        }
    }
}