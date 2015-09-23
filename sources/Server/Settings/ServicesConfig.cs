using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Server.Settings
{
    public class ServicesConfig : ConfigurationElement
    {
        [ConfigurationProperty("tcpService")]
        public TcpServiceConfig TcpService
        {
            get { return (TcpServiceConfig)this["tcpService"]; }
            set { this["tcpService"] = value; }
        }

        [ConfigurationProperty("httpService")]
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
}