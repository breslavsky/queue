using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System;
using System.ServiceModel;

namespace Queue.Reports.ServiceRatingReport
{
    public class ServiceRatingReport : BaseReport
    {
        private readonly ServiceRatingReportSettings settings;

        public ServiceRatingReport(ServiceRatingReportSettings settings)
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
                    throw new FaultException(String.Format("Указанный уровень детализации не поддерживается: {0}", settings.DetailLevel.ToString()));
            }
        }
    }
}