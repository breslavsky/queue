using NPOI.HSSF.UserModel;

namespace Queue.Reports
{
    public interface IQueueReport
    {
        HSSFWorkbook Generate();
    }
}