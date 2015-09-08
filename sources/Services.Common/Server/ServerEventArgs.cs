using Queue.Services.DTO;

namespace Queue.Services.Common
{
    public delegate void ServerEventHandler(object sender, ServerEventArgs e);

    public class ServerEventArgs
    {
        public ClientRequestPlan ClientRequestPlan;
        public ClientRequest ClientRequest;
        public Operator Operator;
        public OperatorPlanMetrics OperatorPlanMetrics;
        public Config Config;
        public Event Event;
    }
}