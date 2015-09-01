using Queue.Services.Hub;
using System;
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

        public event EventHandler<HubQualityDriverArgs> Accepted
        {
            add { throw new System.NotImplementedException(); }
            remove { throw new System.NotImplementedException(); }
        }

        public void Enable(int deviceId)
        {
            throw new System.NotImplementedException();
        }

        public void Disable(int deviceId)
        {
            throw new System.NotImplementedException();
        }
    }
}