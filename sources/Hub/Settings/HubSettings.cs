using Queue.Common;
using System;
using System.Configuration;
using System.Globalization;
using System.Reflection;
using System.Xml;

namespace Queue.Hub.Settings
{
    public class HubSettings : ConfigurationSection
    {
        public const string SectionKey = "hub";

        public HubSettings()
        {
            Services = GetDefaultServicesConfig();
            Drivers = GetDefaultDriversConfig();
            Language = CultureInfo.CurrentCulture.GetLanguage();
        }

        [ConfigurationProperty("drivers")]
        public DriversConfig Drivers
        {
            get { return (DriversConfig)this["drivers"]; }
            set { this["drivers"] = value; }
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

        public override bool IsReadOnly()
        {
            return false;
        }

        private DriversConfig GetDefaultDriversConfig()
        {
            return new DriversConfig()
            {
                Quality = new DriverCollection(),
                Display = new DriverCollection()
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
                    Port = 4511
                },
                TcpService = new TcpServiceConfig()
                {
                    Enabled = true,
                    Host = "localhost",
                    Port = 4512
                }
            };
        }
    }
}