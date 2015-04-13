using Queue.Model;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public abstract class AdditionalServiceRating
    {
        public AdditionalService Service { get; set; }

        public Operator Operator { get; set; }

        public float Quantity { get; set; }
    }
}