using Queue.Services.DTO;

namespace Queue.Services.Contracts.Server
{
    public delegate void QueuePlanEventHandler(object sender, QueuePlanEventArgs e);

    public class QueuePlanEventArgs
    {
        public ClientRequestPlan ClientRequestPlan;
        public ClientRequest ClientRequest;
        public Operator Operator;
        public OperatorPlanMetrics OperatorPlanMetrics;
        public Config Config;
        public Event Event;
    }
}