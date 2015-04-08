using System;

namespace Queue.Model.Common
{
    public abstract class RatingReportSettings
    {
        public ReportDetailLevel DetailLevel { get; set; }

        public int StartYear { get; set; }

        public int FinishYear { get; set; }

        public int StartMonth { get; set; }

        public int FinishMonth { get; set; }

        public int StartDay { get; set; }

        public int FinishDay { get; set; }

        public DateTime TargetDate { get; set; }
    }
}