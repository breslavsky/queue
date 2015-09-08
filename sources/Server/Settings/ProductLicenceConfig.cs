using Queue.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Server.Settings
{
    public class ProductLicenceConfig : ConfigurationElement
    {
        [ConfigurationProperty("licenseType", IsRequired = true)]
        public ProductLicenceType LicenseType
        {
            get { return (ProductLicenceType)this["licenseType"]; }
            set { this["licenseType"] = value; }
        }

        [ConfigurationProperty("serialKey", IsRequired = true)]
        public string SerialKey
        {
            get { return (string)this["serialKey"]; }
            set { this["serialKey"] = value; }
        }

        [ConfigurationProperty("registerKey", IsRequired = true)]
        public string RegisterKey
        {
            get { return (string)this["registerKey"]; }
            set { this["registerKey"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}