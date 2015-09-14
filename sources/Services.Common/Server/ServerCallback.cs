using Queue.Services.DTO;

namespace Queue.Services.Common
{
    public class ServerCallback : ICallback, IServerCallback
    {
        public event ServerEventHandler OnCallClient = delegate { };

        public event ServerEventHandler OnClientRequestUpdated = delegate { };

        public event ServerEventHandler OnCurrentClientRequestPlanUpdated = delegate { };

        public event ServerEventHandler OnOperatorPlanMetricsUpdated = delegate { };

        public event ServerEventHandler OnConfigUpdated = delegate { };

        public event ServerEventHandler OnEvent = delegate { };

        public void CallClient(ClientRequest clientRequest)
        {
            OnCallClient(this, new ServerEventArgs() { ClientRequest = clientRequest });
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
            OnClientRequestUpdated(this, new ServerEventArgs() { ClientRequest = clientRequest });
        }

        public void CurrentClientRequestPlanUpdated(ClientRequestPlan clientRequestPlan, Operator queueOperator)
        {
            OnCurrentClientRequestPlanUpdated(this, new ServerEventArgs() { ClientRequestPlan = clientRequestPlan, Operator = queueOperator });
        }

        public void OperatorPlanMetricsUpdated(OperatorPlanMetrics operatorPlanMetrics)
        {
            OnOperatorPlanMetricsUpdated(this, new ServerEventArgs() { OperatorPlanMetrics = operatorPlanMetrics });
        }

        public void ConfigUpdated(Config config)
        {
            OnConfigUpdated(this, new ServerEventArgs() { Config = config });
        }

        public void Event(Event queueEvent)
        {
            OnEvent(this, new ServerEventArgs() { Event = queueEvent });
        }
    }
}