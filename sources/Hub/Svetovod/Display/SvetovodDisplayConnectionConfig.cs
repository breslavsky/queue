using Junte.Configuration;
using System.ComponentModel;
using System.Configuration;

namespace Queue.Hub.Svetovod
{
    public class SvetovodDisplayConnectionConfig : ConfigurationElement
    {
        [ConfigurationProperty("sysnum")]
        public byte Sysnum
        {
            get { return (byte)this["sysnum"]; }
            set { this["sysnum"] = value; }
        }

        [ConfigurationProperty("type")]
        [TypeConverter(typeof(CaseInsensitiveEnumConfigConverter<SvetovodDisplayType>))]
        public SvetovodDisplayType Type
        {
            get { return (SvetovodDisplayType)this["type"]; }
        }

        [ConfigurationProperty("width")]
        public byte Width
        {
            get { return (byte)this["width"]; }
            set { this["width"] = value; }
        }

        [ConfigurationProperty("height")]
        public byte Height
        {
            get { return (byte)this["height"]; }
            set { this["height"] = value; }
        }

        [ConfigurationProperty("columns")]
        [ConfigurationCollection(typeof(SvetovodDisplayConnectionColumnCollection))]
        public SvetovodDisplayConnectionColumnCollection Columns
        {
            get { return (SvetovodDisplayConnectionColumnCollection)base["columns"]; }
        }
    }
}