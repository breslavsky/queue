using Queue.Model;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public class AdditionalServiceStatisticsDTO
    {
        public AdditionalService Service { get; set; }

        public int Count { get; set; }

        public int Year { get; set; }
    }
}