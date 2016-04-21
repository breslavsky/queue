using System;

namespace Queue.Reports.ExceptionScheduleReport
{
    public class ExceptionScheduleReportProvider : IReportProvider
    {
        private readonly DateTime fromDate;

        public ExceptionScheduleReportProvider(DateTime fromDate)
        {
            this.fromDate = fromDate;
        }

        public IQueueReport GetReport()
        {
            return new ExceptionScheduleReport(fromDate);
        }
    }
}