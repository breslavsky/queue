using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System.ServiceModel;

namespace Queue.Reports.OperatorRatingReport
{
    public class OperatorRatingReport : BaseReport
    {
        private readonly OperatorRatingReportSettings settings;

        public OperatorRatingReport(OperatorRatingReportSettings settings)
        {
            this.settings = settings;
        }

        public override HSSFWorkbook Generate()
        {
            switch (settings.DetailLevel)
            {
                case ReportDetailLevel.Year:
                    return new YearDetailedReport(settings).Generate();

                case ReportDetailLevel.Month:
                    return new MonthDetailedReport(settings).Generate();

                case ReportDetailLevel.Day:
                    return new DayDetailedReport(settings).Generate();

                default:
                    throw new FaultException(string.Format("Указанный уровень детализации не поддерживается: {0}", settings.DetailLevel.ToString()));
            }
        }
    }
}