using Queue.Services.DTO;

namespace Queue.Services.Contracts.Server
{
    public class QueuePlanCallback : IQueuePlanCallback
    {
        public event QueuePlanEventHandler OnCallClient = delegate { };

        public event QueuePlanEventHandler OnClientRequestUpdated = delegate { };

        public event QueuePlanEventHandler OnCurrentClientRequestPlanUpdated = delegate { };

        public event QueuePlanEventHandler OnOperatorPlanMetricsUpdated = delegate { };

        public event QueuePlanEventHandler OnConfigUpdated = delegate { };

        public event QueuePlanEventHandler OnEvent = delegate { };

        public void CallClient(ClientRequest clientRequest)
        {
            OnCallClient(this, new QueuePlanEventArgs() { ClientRequest = clientRequest });
        }

        public void ClientRequestUpdated(ClientRequest clientRequest)
        {
            OnClientRequestUpdated(this, new QueuePlanEventArgs() { ClientRequest = clientRequest });
        }

        public void CurrentClientRequestPlanUpdated(ClientRequestPlan clientRequestPlan, Operator queueOperator)
        {
            OnCurrentClientRequestPlanUpdated(this, new QueuePlanEventArgs() { ClientRequestPlan = clientRequestPlan, Operator = queueOperator });
        }

        public void OperatorPlanMetricsUpdated(OperatorPlanMetrics operatorPlanMetrics)
        {
            OnOperatorPlanMetricsUpdated(this, new QueuePlanEventArgs() { OperatorPlanMetrics = operatorPlanMetrics });
        }

        public void ConfigUpdated(Config config)
        {
            OnConfigUpdated(this, new QueuePlanEventArgs() { Config = config });
        }

        public void Event(Event queueEvent)
        {
            OnEvent(this, new QueuePlanEventArgs() { Event = queueEvent });
        }
    }
}