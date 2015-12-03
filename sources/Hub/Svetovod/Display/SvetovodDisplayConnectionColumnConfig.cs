using Junte.Configuration;
using System.ComponentModel;
using System.Configuration;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayConnectionColumnConfig : ConfigurationElement
    {
        [ConfigurationProperty("index")]
        public byte Index
        {
            get { return (byte)this["index"]; }
            set { this["index"] = value; }
        }

        [ConfigurationProperty("width")]
        public byte Width
        {
            get { return (byte)this["width"]; }
            set { this["width"] = value; }
        }
    }
}