using Junte.WCF.Common;
using Queue.Data.Model.DTO;
using Queue.Services.Interrelate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services
{
    public class ChatHttpServiceHost : ServiceHost
    {
        public ChatHttpServiceHost(DuplexChannelBuilder<IRemoteService> channelBuilder, Administrator currentAdministrator, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            foreach (var description in this.ImplementedContracts.Values)
            {
                description.Behaviors.Add(new ChatHttpServiceProvider(channelBuilder, currentAdministrator));
            }
        }
    }
}
