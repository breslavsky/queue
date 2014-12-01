using Queue.Model;

namespace Queue.Services.Server
{
    public class QueuePlanEventArgs
    {
        public ClientRequestPlan ClientRequestPlan;
        public ClientRequest ClientRequest;
        public Operator Operator;
        public OperatorPlanMetrics OperatorPlanMetrics;
    }
}