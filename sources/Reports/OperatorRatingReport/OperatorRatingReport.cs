using NPOI.HSSF.UserModel;
using Queue.Model.Common;
using System;
using System.ServiceModel;

namespace Queue.Reports.OperatorRatingReport
{
    public class OperatorRatingReport : BaseReport
    {
        private readonly Guid[] operators;
        private readonly OperatorRatingReportSettings settings;
        private readonly ReportDetailLevel detailLavel;

        public OperatorRatingReport(Guid[] operators, ReportDetailLevel detailLevel, OperatorRatingReportSettings settings)
        {
            this.operators = operators;
            this.detailLavel = detailLevel;
            this.settings = settings;
        }

        public override HSSFWorkbook Generate()
        {
            switch (detailLavel)
            {
                case ReportDetailLevel.Year:
                    return new YearDetailedReport(operators, settings).Generate();

                case ReportDetailLevel.Month:
                    return new MonthDetailedReport(operators, settings).Generate();

                case ReportDetailLevel.Day:
                    return new DayDetailedReport(operators, settings).Generate();

                default:
                    throw new FaultException(string.Format("Указанный уровень детализации не поддерживается: {0}", detailLavel.ToString()));
            }
        }
    }
}