using Queue.Common;
using System.Configuration;
using System.Globalization;

namespace Queue.Hub
{
    public class DriverConfig : ConfigurationElement
    {
        [ConfigurationProperty("assembly", IsRequired = true)]
        public string Assembly
        {
            get { return (string)this["assembly"]; }
            set { this["assembly"] = value; }
        }

        [ConfigurationProperty("enabled", IsRequired = true)]
        public bool Enabled
        {
            get { return (bool)this["enabled"]; }
            set { this["enabled"] = value; }
        }

        [ConfigurationProperty("type", IsRequired = true)]
        public string Type
        {
            get { return (string)this["type"]; }
            set { this["type"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class DriversConfig : ConfigurationElement
    {
        [ConfigurationProperty("quality", IsRequired = true)]
        public QualityDriverConfig[] Quality
        {
            get { return (QualityDriverConfig[])this["drivers"]; }
            set { this["drivers"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class HttpServiceConfig : ServiceConfig
    {
    }

    public class HubSettings : ConfigurationSection
    {
        public HubSettings()
        {
            Services = GetDefaultServicesConfig();
            Language = CultureInfo.CurrentCulture.GetLanguage();
        }

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        [ConfigurationProperty("services")]
        public ServicesConfig Services
        {
            get { return (ServicesConfig)this["services"]; }
            set { this["services"] = value; }
        }

        [ConfigurationProperty("drivers")]
        public DriversConfig Drivers
        {
            get { return (DriversConfig)this["drivers"]; }
            set { this["drivers"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
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

    public class ServicesConfig : ConfigurationElement
    {
        [ConfigurationProperty("httpService", IsRequired = true)]
        public HttpServiceConfig HttpService
        {
            get { return (HttpServiceConfig)this["httpService"]; }
            set { this["httpService"] = value; }
        }

        [ConfigurationProperty("tcpService", IsRequired = true)]
        public TcpServiceConfig TcpService
        {
            get { return (TcpServiceConfig)this["tcpService"]; }
            set { this["tcpService"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class TcpServiceConfig : ServiceConfig
    {
    }

    public class QualityDriverConfig : DriverConfig
    {
    }
}