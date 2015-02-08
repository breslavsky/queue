using Junte.Data.NHibernate;
using System.Configuration;

namespace Queue.Metric
{
    internal class MetricSettings : ConfigurationSection
    {
        [ConfigurationProperty("database")]
        public DatabaseSettings Database
        {
            get { return (DatabaseSettings)this["database"]; }
            set { this["database"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}