using Queue.Model;
using System;

namespace Queue.Services.Server
{
    public class OperatorPlanMetrics
    {
        public OperatorPlanMetrics(Operator queueOperator)
        {
            Operator = queueOperator;
        }

        public Operator Operator { get; private set; }

        public int LastPosition { get; set; }

        public int Standing { get; set; }

        public TimeSpan DailyWorkload { get; set; }

        public TimeSpan PlaningWorkload { get; set; }

        public void Reset()
        {
            LastPosition = 0;
            Standing = 0;
            PlaningWorkload = TimeSpan.Zero;
        }
    }
}