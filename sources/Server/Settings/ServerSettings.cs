using Junte.Data.NHibernate;
using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.Server.Settings
{
    public class ServerSettings : ConfigurationSection
    {
        public const string SectionKey = "server";

        private const string DefaultRegisterKey = "0000-0000-0000-0000-0000";
        private const string DefaultSerialKey = "0000-0000-0000-0000-0000";

        public ServerSettings()
        {
            Database = new DatabaseSettings()
            {
                Server = "localhost",
                Name = "queue",
                Type = DatabaseType.MsSql,
                Integrated = true
            };

            Services = new ServicesConfig()
            {
                HttpService = new HttpServiceConfig()
                {
                    Enabled = false,
                    Host = "localhost",
                    Port = 4506
                },
                TcpService = new TcpServiceConfig()
                {
                    Enabled = true,
                    Host = "localhost",
                    Port = 4505
                }
            };

            Licence = new ProductLicenceConfig()
            {
                LicenseType = ProductLicenceType.NonCommercial,
                SerialKey = DefaultSerialKey,
                RegisterKey = DefaultRegisterKey
            };
            Language = CultureInfo.CurrentCulture.GetLanguage();
        }

        [ConfigurationProperty("database")]
        public DatabaseSettings Database
        {
            get { return (DatabaseSettings)this["database"]; }
            set { this["database"] = value; }
        }

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        [ConfigurationProperty("licence")]
        public ProductLicenceConfig Licence
        {
            get { return (ProductLicenceConfig)this["licence"]; }
            set { this["licence"] = value; }
        }

        [ConfigurationProperty("services")]
        public ServicesConfig Services
        {
            get { return (ServicesConfig)this["services"]; }
            set { this["services"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}