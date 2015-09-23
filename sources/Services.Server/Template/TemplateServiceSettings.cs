using Queue.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Queue.Services.Server.Settings
{
    public class TemplateServiceSettings : ConfigurationSection
    {
        public const string SectionKey = "templateService";

        [ConfigurationProperty("folder")]
        public string Folder
        {
            get { return (string)this["folder"]; }
            set { this["folder"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}