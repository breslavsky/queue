using Junte.Data.NHibernate;
using Junte.Translation;
using NHibernate.Mapping.Attributes;
using NHibernate.Validator.Constraints;
using Queue.Common;
using Queue.Model.Common;
using System;

namespace Queue.Model
{
    [Class(Table = "queue_plan_report", DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public class QueuePlanReport : IdentifiedEntity
    {
        private const int ReportLength = 1024 * 500;

        [ManyToOne(ClassType = typeof(ClientRequest), Column = "ClientRequestId", ForeignKey = "QueuePlanReportToClientRequestReference")]
        public virtual ClientRequest ClientRequest { get; set; }

        [Property(Length = ReportLength)]
        public virtual string Report { get; set; }
    }
}