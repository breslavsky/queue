using Queue.Services.DTO;

namespace Queue.Services.Common
{
    public class ServerCallback : IServerCallback
    {
        public event ServerEventHandler OnCallClient
        {
            add { callClientHandler += value; }
            remove { callClientHandler -= value; }
        }

        public event ServerEventHandler OnClientRequestUpdated
        {
            add { clientRequestUpdatedHandler += value; }
            remove { clientRequestUpdatedHandler -= value; }
        }

        public event ServerEventHandler OnCurrentClientRequestPlanUpdated
        {
            add { currentClientRequestPlanUpdatedHandler += value; }
            remove { currentClientRequestPlanUpdatedHandler -= value; }
        }

        public event ServerEventHandler OnOperatorPlanMetricsUpdated
        {
            add { operatorPlanMetricsUpdatedHandler += value; }
            remove { operatorPlanMetricsUpdatedHandler -= value; }
        }

        public event ServerEventHandler OnConfigUpdated
        {
            add { configUpdatedHandler += value; }
            remove { configUpdatedHandler -= value; }
        }

        public event ServerEventHandler OnEvent
        {
            add { eventHandler += value; }
            remove { eventHandler -= value; }
        }

        private event ServerEventHandler callClientHandler;

        private event ServerEventHandler clientRequestUpdatedHandler;

        private event ServerEventHandler currentClientRequestPlanUpdatedHandler;

        private event ServerEventHandler operatorPlanMetricsUpdatedHandler;

        private event ServerEventHandler configUpdatedHandler;

        private event ServerEventHandler eventHandler;

        public void CallClient(ClientRequest clientRequest)
        {
            if (callClientHandler != null)
            {
                callClientHandler(this, new ServerEventArgs() { ClientRequest = clientRequest });
            }
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
            if (clientRequestUpdatedHandler != null)
            {
                clientRequestUpdatedHandler(this, new ServerEventArgs() { ClientRequest = clientRequest });
            }
        }

        public void CurrentClientRequestPlanUpdated(ClientRequestPlan clientRequestPlan, Operator queueOperator)
        {
            if (currentClientRequestPlanUpdatedHandler != null)
            {
                currentClientRequestPlanUpdatedHandler(this, new ServerEventArgs() { ClientRequestPlan = clientRequestPlan, Operator = queueOperator });
            }
        }

        public void OperatorPlanMetricsUpdated(OperatorPlanMetrics operatorPlanMetrics)
        {
            if (operatorPlanMetricsUpdatedHandler != null)
            {
                operatorPlanMetricsUpdatedHandler(this, new ServerEventArgs() { OperatorPlanMetrics = operatorPlanMetrics });
            }
        }

        public void ConfigUpdated(Config config)
        {
            if (configUpdatedHandler != null)
            {
                configUpdatedHandler(this, new ServerEventArgs() { Config = config });
            }
        }

        public void Event(Event queueEvent)
        {
            if (eventHandler != null)
            {
                eventHandler(this, new ServerEventArgs() { Event = queueEvent });
            }
        }
    }
}