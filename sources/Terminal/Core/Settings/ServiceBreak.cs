using System;
using System.Configuration;

namespace Queue.Terminal.Core.Settings
{
    public class ServiceBreak : ConfigurationElement
    {
        [ConfigurationProperty("from")]
        public TimeSpan From
        {
            get { return (TimeSpan)this["from"]; }
            set { this["from"] = value; }
        }

        [ConfigurationProperty("to")]
        public TimeSpan To
        {
            get { return (TimeSpan)this["to"]; }
            set { this["to"] = value; }
        }

        [ConfigurationProperty("message")]
        public string Message
        {
            get { return (string)this["message"]; }
            set { this["message"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}