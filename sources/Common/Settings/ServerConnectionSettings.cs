using System;
using System.Configuration;

namespace Queue.Common
{
    public class ServerConnectionSettings : ConfigurationSection
    {
        [ConfigurationProperty("endpoint")]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("password")]
        public string Password
        {
            get { return (string)this["password"]; }
            set { this["password"] = value; }
        }

        [ConfigurationProperty("user")]
        public Guid User
        {
            get { return (Guid)this["user"]; }
            set { this["user"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}