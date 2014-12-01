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
        void CallClient(ClientRequestFull clientRequest);

        [OperationContract(IsOneWay = true)]
        void ClientRequestUpdated(ClientRequestFull clientRequest);

        [OperationContract(IsOneWay = true)]
        void CurrentClientRequestPlanUpdated(ClientRequestPlanFull clientRequestPlan, OperatorFull queueOperator);

        [OperationContract(IsOneWay = true)]
        void OperatorPlanMetricsUpdated(OperatorPlanMetricsFull operatorPlanMetrics);

        [OperationContract(IsOneWay = true)]
        void ConfigUpdated(ConfigFull config);
    }
}