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

        private event ServerEventHandler callClientHandler;

        private event ServerEventHandler clientRequestUpdatedHandler;

        private event ServerEventHandler currentClientRequestPlanUpdatedHandler;

        private event ServerEventHandler operatorPlanMetricsUpdatedHandler;

        private event ServerEventHandler configUpdatedHandler;

        public void CallClient(ClientRequestFull clientRequest)
        {
            if (callClientHandler != null)
            {
                callClientHandler(this, new ServerEventArgs()
                {
                    ClientRequest = clientRequest
                });
            }
        }

        public void ClientRequestUpdated(ClientRequestFull clientRequest)
        {
            if (clientRequestUpdatedHandler != null)
            {
                clientRequestUpdatedHandler(this, new ServerEventArgs()
                {
                    ClientRequest = clientRequest
                });
            }
        }

        public void CurrentClientRequestPlanUpdated(ClientRequestPlanFull clientRequestPlan, OperatorFull queueOperator)
        {
            if (currentClientRequestPlanUpdatedHandler != null)
            {
                currentClientRequestPlanUpdatedHandler(this, new ServerEventArgs()
                {
                    ClientRequestPlan = clientRequestPlan,
                    Operator = queueOperator
                });
            }
        }

        public void OperatorPlanMetricsUpdated(OperatorPlanMetricsFull operatorPlanMetrics)
        {
            if (operatorPlanMetricsUpdatedHandler != null)
            {
                operatorPlanMetricsUpdatedHandler(this, new ServerEventArgs()
                {
                    OperatorPlanMetrics = operatorPlanMetrics
                });
            }
        }

        public void ConfigUpdated(ConfigFull config)
        {
            if (configUpdatedHandler != null)
            {
                configUpdatedHandler(this, new ServerEventArgs()
                {
                    Config = config
                });
            }
        }
    }
}