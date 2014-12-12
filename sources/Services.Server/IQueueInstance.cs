using Queue.Model;
using System;

namespace Queue.Services.Server
{
    public interface IQueueInstance : IDisposable
    {
        event EventHandler<QueueInstanceEventArgs> OnCallClient;

        event EventHandler<QueueInstanceEventArgs> OnClientRequestUpdated;

        event EventHandler<QueueInstanceEventArgs> OnCurrentClientRequestPlanUpdated;

        event EventHandler<QueueInstanceEventArgs> OnOperatorPlanMetricsUpdated;

        event EventHandler<QueueInstanceEventArgs> OnConfigUpdated;

        event EventHandler<QueueInstanceEventArgs> OnEvent;

        QueuePlan TodayQueuePlan { get; }

        void CallClient(ClientRequest clientRequest);

        void ClientRequestUpdated(ClientRequest clientRequest);

        void ConfigUpdated(Config config);

        void Event(Event queueEvent);
    }
}