using Junte.Data.NHibernate;
using NHibernate.Mapping.Attributes;

namespace Queue.Model
{
    [Class(Table = "metric", Abstract = true, DynamicUpdate = true, Lazy = false)]
    [Cache(Usage = CacheUsage.ReadWrite)]
    public abstract class Metric : IdentifiedEntity
    {
        #region properties

        [Property(Index = "Year")]
        public virtual int Year { get; set; }

        [Property(Index = "Month")]
        public virtual int Month { get; set; }

        [Property(Index = "Day")]
        public virtual int Day { get; set; }

        [Property(Index = "Hour")]
        public virtual int Hour { get; set; }

        [Property(Index = "Minute")]
        public virtual int Minute { get; set; }

        [Property(Index = "Second")]
        public virtual int Second { get; set; }

        #endregion properties
    }
}