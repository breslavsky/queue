using Queue.Services.DTO;
using System.ServiceModel;

namespace Queue.Services.Common
{
    [ServiceKnownTypeAttribute(typeof(DefaultConfig))]
    [ServiceKnownTypeAttribute(typeof(CouponConfig))]
    [ServiceKnownTypeAttribute(typeof(SMTPConfig))]
    [ServiceKnownTypeAttribute(typeof(PortalConfig))]
    [ServiceKnownTypeAttribute(typeof(MediaConfig))]
    [ServiceKnownTypeAttribute(typeof(TerminalConfig))]
    [ServiceKnownTypeAttribute(typeof(DesignConfig))]
    [ServiceKnownTypeAttribute(typeof(NotificationConfig))]
    public interface IServerCallback
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