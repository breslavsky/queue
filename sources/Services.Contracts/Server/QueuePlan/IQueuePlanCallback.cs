using Queue.Services.DTO;
using System.ServiceModel;

namespace Queue.Services.Contracts.Server
{
    [ServiceKnownType(typeof(DefaultConfig))]
    [ServiceKnownType(typeof(CouponConfig))]
    [ServiceKnownType(typeof(SMTPConfig))]
    [ServiceKnownType(typeof(PortalConfig))]
    [ServiceKnownType(typeof(MediaConfig))]
    [ServiceKnownType(typeof(TerminalConfig))]
    [ServiceKnownType(typeof(DesignConfig))]
    [ServiceKnownType(typeof(NotificationConfig))]
    public interface IQueuePlanCallback
    {
        [OperationContract(IsOneWay = true)]
        void CallClient(ClientRequest clientRequest);

        [OperationContract(IsOneWay = true)]
        void ClientRequestUpdated(ClientRequest clientRequest);

        [OperationContract(IsOneWay = true)]
        void CurrentClientRequestPlanUpdated(ClientRequestPlan clientRequestPlan, Operator queueOperator);

        [OperationContract(IsOneWay = true)]
        void OperatorPlanMetricsUpdated(OperatorPlanMetrics operatorPlanMetrics);

        [OperationContract(IsOneWay = true)]
        void ConfigUpdated(Config config);

        [OperationContract(IsOneWay = true)]
        void Event(Event queueEvent);
    }
}