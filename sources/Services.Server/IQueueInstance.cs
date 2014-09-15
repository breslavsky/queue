using Queue.Model;
using System;

namespace Queue.Services.Server
{
    public interface IQueueInstance : IDisposable
    {
        event QueueInstanceEventHandler OnCallClient;

        event QueueInstanceEventHandler OnClientRequestUpdated;

        event QueueInstanceEventHandler OnCurrentClientRequestPlanUpdated;

        event QueueInstanceEventHandler OnOperatorPlanMetricsUpdated;

        event QueueInstanceEventHandler OnConfigUpdated;

        event QueueInstanceEventHandler OnEvent;

        QueuePlan TodayQueuePlan { get; }

        void CallClient(ClientRequest clientRequest);

        void ClientRequestUpdated(ClientRequest clientRequest);

        void ConfigUpdated(Config config);

        void Event(Event queueEvent);
    }
}