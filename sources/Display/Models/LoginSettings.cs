using Queue.Common;
using System;
using System.Configuration;

namespace Queue.Display.Models
{
    public class LoginSettings : ConfigurationSection
    {
        public const string SectionKey = "loginForm";

        [ConfigurationProperty("isRemember")]
        public bool IsRemember
        {
            get { return (bool)this["isRemember"]; }
            set { this["isRemember"] = value; }
        }

        [ConfigurationProperty("accent")]
        public string Accent
        {
            get { return (string)this["accent"]; }
            set { this["accent"] = value; }
        }

        [ConfigurationProperty("language")]
        public Language Language
        {
            get { return (Language)this["language"]; }
            set { this["language"] = value; }
        }

        [ConfigurationProperty("endpoint")]
        public string Endpoint
        {
            get { return (string)this["endpoint"]; }
            set { this["endpoint"] = value; }
        }

        [ConfigurationProperty("workplaceId")]
        public Guid WorkplaceId
        {
            get { return (Guid)this["workplaceId"]; }
            set { this["workplaceId"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}