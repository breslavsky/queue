using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue.Services.Portal
{
    internal class PortalOperatorServiceProvider : IInstanceProvider, IContractBehavior
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private Administrator currentUser;

        public PortalOperatorServiceProvider(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new PortalOperatorService(channelBuilder, currentUser);
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