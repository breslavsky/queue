﻿using Queue.Common;
using System;
using System.Configuration;
using System.Globalization;

namespace Queue.Display.Models
{
    public class DisplayLoginSettings : ConfigurationSection
    {
        public const string SectionKey = "loginForm";

        public DisplayLoginSettings()
        {
            Endpoint = "net.tcp://queue:4505";
        }

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

        public DisplayLoginSettings()
        {
            Language = CultureInfo.CurrentCulture.GetLanguage();
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}