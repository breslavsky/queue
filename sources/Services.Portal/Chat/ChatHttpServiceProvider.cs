using Junte.WCF.Common;
using Queue.Data.Model.DTO;
using Queue.Services.Interrelate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services
{
    internal class ChatHttpServiceProvider : IInstanceProvider, IContractBehavior
    {
        private DuplexChannelBuilder<IRemoteService> channelBuilder;
        private Administrator currentAdministrator;

        public ChatHttpServiceProvider(DuplexChannelBuilder<IRemoteService> channelBuilder, Administrator currentAdministrator)
        {
            this.channelBuilder = channelBuilder;
            this.currentAdministrator = currentAdministrator;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new ChatHttpService(channelBuilder, currentAdministrator);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }

        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
    }
}
