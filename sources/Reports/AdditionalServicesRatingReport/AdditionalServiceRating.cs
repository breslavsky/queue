using Queue.Model;
using System;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    public abstract class AdditionalServiceRating
    {
        public AdditionalService Service { get; set; }

        public Operator Operator { get; set; }

        public Single Quantity { get; set; }
    }
}