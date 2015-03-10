using Junte.Data.NHibernate;
using Queue.Common;
using System.Configuration;

namespace Queue.Server
{
    public class ServerSettings : AbstractSettings
    {
        [ConfigurationProperty("debug")]
        public bool Debug
        {
            get { return (bool)this["debug"]; }
            set { this["debug"] = value; }
        }

        [ConfigurationProperty("database")]
        public DatabaseSettings Database
        {
            get { return (DatabaseSettings)this["database"]; }
            set { this["database"] = value; }
        }

        [ConfigurationProperty("services")]
        public ServicesConfig Services
        {
            get { return (ServicesConfig)this["services"]; }
            set { this["services"] = value; }
        }

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        public ServerSettings()
        {
            Database = GetDefaultDatabaseSettings();
            Services = GetDefaultServicesConfig();
            Debug = true;
        }

        public override bool IsReadOnly()
        {
            return false;
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

        private ServicesConfig GetDefaultServicesConfig()
        {
            return new ServicesConfig()
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
        }
    }

    public class ServicesConfig : ConfigurationElement
    {
        [ConfigurationProperty("tcpService", IsRequired = true)]
        public TcpServiceConfig TcpService
        {
            get { return (TcpServiceConfig)this["tcpService"]; }
            set { this["tcpService"] = value; }
        }

        [ConfigurationProperty("httpService", IsRequired = true)]
        public HttpServiceConfig HttpService
        {
            get { return (HttpServiceConfig)this["httpService"]; }
            set { this["httpService"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class ServiceConfig : ConfigurationElement
    {
        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("host", IsRequired = true)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class TcpServiceConfig : ServiceConfig
    {
    }

    public class HttpServiceConfig : ServiceConfig
    {
    }
}