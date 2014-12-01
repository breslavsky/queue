using Queue.Services.DTO;

namespace Queue.Services.Common
{
    public delegate void ServerEventHandler(object sender, ServerEventArgs e);

    public class ServerEventArgs
    {
        public ClientRequestPlanFull ClientRequestPlan;
        public ClientRequestFull ClientRequest;
        public OperatorFull Operator;
        public OperatorPlanMetricsFull OperatorPlanMetrics;
        public ConfigFull Config;
    }
}