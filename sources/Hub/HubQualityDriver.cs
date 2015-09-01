using Queue.Server;
using Queue.Services.Hub;
using System.Configuration;

namespace Queue.Hub
{
    public class CustomQualityDriverConfig : QualityDriverConfig
    {
        [ConfigurationProperty("port", IsRequired = true)]
        public int port
        {
            get { return (int)this["port"]; }
            set { this["port"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }

    public class HubQualityDriver : IHubQualityDriver
    {
        private CustomQualityDriverConfig config;

        public HubQualityDriver(CustomQualityDriverConfig config)
        {
            this.config = config;
        }
    }
}