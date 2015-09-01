using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue.Services.Hub
{
    internal class HubQualityServiceProvider : IInstanceProvider, IContractBehavior
    {
        public HubQualityServiceProvider()
        {
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            var instance = new HubQualityService();
            ServiceLocator.Current.GetInstance<IUnityContainer>().BuildUp(instance);
            return instance;
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