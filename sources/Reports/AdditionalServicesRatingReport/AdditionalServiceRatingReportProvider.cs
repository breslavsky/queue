using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System;
using System.ServiceModel;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public class AdditionalServiceRatingReportProvider : IReportProvider
    {
        private readonly AdditionalServicesRatingReportSettings settings;

        public AdditionalServiceRatingReportProvider(AdditionalServicesRatingReportSettings settings)
        {
            this.settings = settings;
        }

        public IQueueReport GetReport()
        {
            switch (settings.DetailLevel)
            {
                case ReportDetailLevel.Year:
                    return new YearDetailedReport(settings);

                case ReportDetailLevel.Month:
                    return new MonthDetailedReport(settings);

                case ReportDetailLevel.Day:
                    return new DayDetailedReport(settings);

                default:
                    throw new FaultException(String.Format("Указанный уровень детализации не поддерживается: {0}", settings.DetailLevel));
            }
        }
    }
}