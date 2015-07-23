namespace Queue.Common
{
    public struct Product
    {
        public const string Name = "Junte Queue System";
        public const string Copyright = "Copyright ©  2015";
        public const string Company = "Junte Ltd.";
        public const string Trademark = "Junte";
        public const string Language = "ru";

        public struct Version
        {
            public const string Major = "1";
            public const string Minor = "5";

            public const string Full = Major + "." + Minor;
        }

        #region componenets

        public struct Database
        {
            public const string Name = "Queue Database";
            public const string Description = "";
            public const string Build = "5";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "7ED0004B-0B6F-4BEE-8756-265D2A0B5C97";
        }

        public struct Simulator
        {
            public const string Name = "Queue Simulator";
            public const string Description = "";
            public const string Build = "0";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "48BD3CA5-D7C6-473E-8EF3-3AC0116CBD46";
        }

        public struct Server
        {
            public const string Name = "Queue Server";
            public const string Description = "";
            public const string Build = "10";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "933F65E8-910F-4429-BA0B-16B3CF43E620";
        }

        public struct Portal
        {
            public const string Name = "Queue Portal";
            public const string Description = "";
            public const string Build = "7";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "F337BE00-134F-4C83-B316-BCBA4491B940";
        }

        public struct Metric
        {
            public const string Name = "Queue Metric";
            public const string Description = "";
            public const string Build = "5";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "D16407F8-CE16-4FCC-A96F-78350D7CD525";
        }

        public struct Media
        {
            public const string Name = "Queue Media";
            public const string Description = "";
            public const string Build = "5";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "D16407F8-CE16-4FCC-A96F-78350D7CD525";
        }

        public struct Administrator
        {
            public const string Name = "Queue Administrator";
            public const string Description = "";
            public const string Build = "13";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "06C4B3D8-20FF-4D84-B14D-30A8BA06CDE6";
        }

        public struct Operator
        {
            public const string Name = "Queue Operator";
            public const string Description = "";
            public const string Build = "5";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "1B26F703-3003-47F7-9204-4A11361F75F5";
        }

        public struct Terminal
        {
            public const string Name = "Queue Terminal";
            public const string Description = "";
            public const string Build = "7";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "940AE5A5-2825-4F85-A62C-66A97D9E4C06";
        }

        public struct Notification
        {
            public const string Name = "Queue Notification";
            public const string Description = "";
            public const string Build = "0";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "FEB87B29-F2BA-45D7-989D-8C8610088FAB";
        }

        public struct Display
        {
            public const string Name = "Queue Display";
            public const string Description = "";
            public const string Build = "0";
            public const string Version = Product.Version.Full + "." + Build;
            public const string Guid = "3E1140C5-D566-462A-9596-A992185626FE";
        }

        #endregion componenets
    }
}