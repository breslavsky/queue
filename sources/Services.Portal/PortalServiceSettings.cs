using Queue.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Portal.Settings
{
    public class PortalServiceSettings : ConfigurationSection
    {
        public const string SectionKey = "portalService";

        [ConfigurationProperty("contentFolder")]
        public string ContentFolder
        {
            get { return (string)this["contentFolder"]; }
            set { this["contentFolder"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}