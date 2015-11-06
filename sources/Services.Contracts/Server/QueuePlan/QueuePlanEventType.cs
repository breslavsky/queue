namespace Queue.Services.Contracts.Server
{
    public enum QueuePlanEventType
    {
        CallClient,
        ClientRequestUpdated,
        CurrentClientRequestPlanUpdated,
        OperatorPlanMetricsUpdated,
        ConfigUpdated,
        Event
    }
}