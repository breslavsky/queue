using Microsoft.Practices.Unity;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue
{
    public class UnityInstanceProvider
      : IInstanceProvider, IContractBehavior
    {
        private readonly IUnityContainer container;

        public UnityInstanceProvider(IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            this.container = container;
        }

        #region IInstanceProvider Members

        public object GetInstance(InstanceContext instanceContext,
          Message message)
        {
            return this.GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return this.container.Resolve(
              instanceContext.Host.Description.ServiceType);
        }

        public void ReleaseInstance(InstanceContext instanceContext,
          object instance)
        {
        }

        #endregion IInstanceProvider Members

        #region IContractBehavior Members

        public void AddBindingParameters(
          ContractDescription contractDescription,
          ServiceEndpoint endpoint,
          BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(
          ContractDescription contractDescription,
          ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(
          ContractDescription contractDescription,
          ServiceEndpoint endpoint,
          DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = this;
        }

        public void Validate(
          ContractDescription contractDescription,
          ServiceEndpoint endpoint)
        {
        }

        #endregion IContractBehavior Members
    }
}