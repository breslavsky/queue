using System;

namespace Queue.Model.Common
{
    public class ServiceRatingReportSettings : RatingReportSettings
    {
        public Guid[] Services { get; set; }

        public bool IsServiceTypes { get; set; }
    }
}