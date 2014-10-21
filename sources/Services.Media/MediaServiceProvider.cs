using Junte.WCF.Common;
using Queue.Services.Contracts;
using Queue.Services.DTO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue.Services.Media
{
    internal class MediaServiceProvider : IInstanceProvider, IContractBehavior
    {
        private DuplexChannelBuilder<IServerTcpService> channelBuilder;
        private Administrator currentUser;
        private string folder;

        public MediaServiceProvider(DuplexChannelBuilder<IServerTcpService> channelBuilder, Administrator currentUser, string folder)
        {
            this.channelBuilder = channelBuilder;
            this.currentUser = currentUser;
            this.folder = folder;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new MediaService(channelBuilder, currentUser, folder);
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