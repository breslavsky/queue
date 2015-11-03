using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Queue.Services.Contracts;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Queue.Services.Server
{
    public class CORSEnablingBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        public void AddBindingParameters(
          ServiceEndpoint endpoint,
          BindingParameterCollection bindingParameters) { }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(
              new CORSHeaderInjectingMessageInspector()
            );
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }

        public override Type BehaviorType { get { return typeof(CORSEnablingBehavior); } }

        protected override object CreateBehavior()
        {
            return new CORSEnablingBehavior();
        }

        private class CORSHeaderInjectingMessageInspector : IDispatchMessageInspector
        {
            public object AfterReceiveRequest(
              ref Message request,
              IClientChannel channel,
              InstanceContext instanceContext)
            {
                return null;
            }

            public void BeforeSendReply(ref Message reply, object correlationState)
            {
                var httpRequestMessage = new HttpResponseMessageProperty();
                httpRequestMessage.Headers.Add("Access-Control-Allow-Origin", "*");
                reply.Properties.Add(HttpResponseMessageProperty.Name, httpRequestMessage);

                reply.Headers.Add(MessageHeader.CreateHeader("Access-Control-Allow-Origin", "*", ""));
            }
        }
    }

    internal class ServerHttpServiceProvider : IInstanceProvider, IContractBehavior
    {
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return GetInstance(instanceContext);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return new ServerHttpService();
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