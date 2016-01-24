using System;

namespace Queue.Reports.ClientRequestReport
{
    public class ClientRequestReportProvider : IReportProvider
    {
        private readonly Guid clientRequestId;

        public ClientRequestReportProvider(Guid clientRequestId)
            : base()
        {
            this.clientRequestId = clientRequestId;
        }

        public IQueueReport GetReport()
        {
            return new ClientRequestReport(clientRequestId);
        }
    }
}