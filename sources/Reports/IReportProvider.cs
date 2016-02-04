namespace Queue.Reports
{
    public interface IReportProvider
    {
        IQueueReport GetReport();
    }
}