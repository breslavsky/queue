using Queue.Model;
using System;

namespace Queue.Services.Server
{
    public class ClientRequestPlan
    {
        public ClientRequestPlan(ClientRequest clientRequest, TimeSpan startTime, TimeSpan finishTime)
        {
            ClientRequest = clientRequest;
            StartTime = startTime;
            FinishTime = finishTime;
        }

        public ClientRequest ClientRequest { get; private set; }

        public TimeSpan FinishTime { get; private set; }

        public int Position { get; set; }

        public TimeSpan StartTime { get; private set; }

        public override string ToString()
        {
            return ClientRequest.ToString();
        }
    }
}