﻿using Queue.Model.Common;
using System;
using System.Runtime.Serialization;

namespace Queue.Services.DTO
{
    [DataContract]
    [KnownType(typeof(DefaultConfig))]
    [KnownType(typeof(DesignConfig))]
    public class Config : Entity
    {
        [DataMember]
        public ConfigType Type { get; set; }
    }

    [DataContract]
    public class DefaultConfig : Config
    {
        [DataMember]
        public string QueueName { get; set; }

        [DataMember]
        public TimeSpan WorkStartTime { get; set; }

        [DataMember]
        public TimeSpan WorkFinishTime { get; set; }

        [DataMember]
        public int MaxClientRequests { get; set; }

        [DataMember]
        public int MaxRenderingTime { get; set; }

        [DataMember]
        public bool IsDebug { get; set; }
    }

    [DataContract]
    public class DesignConfig : Config
    {
        [DataMember]
        public byte[] LogoSmall { get; set; }
    }

    [DataContract]
    public class CouponConfig : Config
    {
        [DataMember]
        public CouponSection Sections { get; set; }
    }

    [DataContract]
    public class SMTPConfig : Config
    {
        [DataMember]
        public string Server { get; set; }

        [DataMember]
        public int Port { get; set; }

        [DataMember]
        public bool EnableSsl { get; set; }

        [DataMember]
        public string User { get; set; }

        [DataMember]
        public string Password { get; set; }

        [DataMember]
        public string From { get; set; }
    }

    [DataContract]
    public class PortalConfig : Config
    {
        [DataMember]
        public bool CurrentDayRecording { get; set; }
    }

    [DataContract]
    public class MediaConfig : Config
    {
        [DataMember]
        public string ServiceUrl { get; set; }

        [DataMember]
        public string Ticker { get; set; }

        [DataMember]
        public int TickerSpeed { get; set; }
    }

    [DataContract]
    public class MediaConfigFile : IdentifiedEntity
    {
        [DataMember]
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    [DataContract]
    public class TerminalConfig : Config
    {
        [DataMember]
        public int PIN { get; set; }

        [DataMember]
        public bool CurrentDayRecording { get; set; }

        [DataMember]
        public int Columns { get; set; }

        [DataMember]
        public int Rows { get; set; }

        [DataMember]
        public TerminalPages Pages { get; set; }

        [DataMember]
        public TerminalPages StartPage { get; set; }
    }

    [DataContract]
    public class NotificationConfig : Config
    {
        [DataMember]
        public virtual int ClientRequestsLength { get; set; }
    }
}