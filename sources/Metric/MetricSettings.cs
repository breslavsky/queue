using Junte.Data.NHibernate;
using Queue.Common;
using System.Configuration;

namespace Queue.Metric
{
    public class MetricSettings : AbstractSettings
    {
        public MetricSettings()
        {
            Database = GetDefaultDatabaseSettings();
        }

        [ConfigurationProperty("database")]
        public DatabaseSettings Database
        {
            get { return (DatabaseSettings)this["database"]; }
            set { this["database"] = value; }
        }

        private DatabaseSettings GetDefaultDatabaseSettings()
        {
            return new DatabaseSettings()
            {
                Server = "localhost",
                Name = "queue",
                Type = DatabaseType.MsSql,
                Integrated = true
            };
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}