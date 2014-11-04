using Queue.Model;
using System;

namespace Queue.Reports.OperatorRatingReport
{
    public class OperatorRating
    {
        public Operator Operator { get; set; }

        public int Total { get; set; }

        public int Live { get; set; }

        public int Early { get; set; }

        public int Waiting { get; set; }

        public int Absence { get; set; }

        public int Rendered { get; set; }

        public int Canceled { get; set; }

        public TimeSpan RenderTime { get; set; }

        public TimeSpan WaitingTime { get; set; }

        public int SubjectsTotal { get; set; }

        public int SubjectsLive { get; set; }

        public int SubjectsEarly { get; set; }
    }
}