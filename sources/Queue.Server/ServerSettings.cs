using Junte.Data.NHibernate;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue
{
    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ServicesSettings
    {
        public TcpServiceSettings TcpService { get; set; }

        public HttpServiceSettings HttpService { get; set; }
    }

    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ServiceSettings
    {
        public bool Enabled { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }
    }

    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class TcpServiceSettings : ServiceSettings
    {
        public TcpServiceSettings()
            : base()
        {
            Port = 4505;
        }
    }

    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class HttpServiceSettings : ServiceSettings
    {
        public HttpServiceSettings()
            : base()
        {
            Port = 4515;
        }
    }

    [SettingsSerializeAs(SettingsSerializeAs.Xml)]
    public class ServerSettings
    {
        public DatabaseSettings Database { get; set; }

        public ServicesSettings Services { get; set; }
    }
}