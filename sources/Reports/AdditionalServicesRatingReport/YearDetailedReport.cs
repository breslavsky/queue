using Queue.Common;
using Queue.Model.Common;
using System;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    internal class YearDetailedReport : BaseDetailedReport
    {
        public YearDetailedReport(AdditionalServicesRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfYear(settings.StartYear);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfYear(settings.FinishYear);
        }
    }
}