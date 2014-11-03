using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System;

namespace Queue.Reports.OperatorRatingReport
{
    public class OperatorRatingReport : BaseReport
    {
        private readonly Guid[] servicesIds;
        private readonly OperatorRatingReportSettings settings;
        private readonly ReportDetailLevel detailLavel;

        public OperatorRatingReport(Guid[] services, ReportDetailLevel detailLevel, OperatorRatingReportSettings settings)
        {
            this.servicesIds = services;
            this.detailLavel = detailLevel;
            this.settings = settings;
        }

        public override HSSFWorkbook Generate()
        {
            //switch (detailLavel)
            //{
            //    case ServiceRatingReportDetailLavel.Year:
            //        return new YearDetailedReport(servicesIds, settings).Generate();

            //    case ServiceRatingReportDetailLavel.Month:
            //        return new MonthDetailedReport(servicesIds, settings).Generate();

            //    case ServiceRatingReportDetailLavel.Day:

            //        return new DayDetailedReport(servicesIds, settings).Generate();

            //    default:
            //        throw new FaultException(string.Format("Указанный уровень детализации не поддерживается: {0}", detailLavel.ToString()));
            //}

            return null;
        }
    }
}