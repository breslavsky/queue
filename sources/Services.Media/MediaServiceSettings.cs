using Queue.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Media.Settings
{
    public class MediaServiceSettings : ConfigurationSection
    {
        public const string SectionKey = "mediaService";

        [ConfigurationProperty("mediaFolder")]
        public string MediaFolder
        {
            get { return (string)this["mediaFolder"]; }
            set { this["mediaFolder"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}