using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System.ServiceModel;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public class AdditionalServiceRatingReport : BaseReport
    {
        private readonly AdditionalServicesRatingReportSettings settings;

        public AdditionalServiceRatingReport(AdditionalServicesRatingReportSettings settings)
        {
            this.settings = settings;
        }

        protected override HSSFWorkbook InternalGenerate()
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
                    throw new FaultException(string.Format("Указанный уровень детализации не поддерживается: {0}", settings.DetailLevel));
            }
        }
    }
}