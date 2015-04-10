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
            public const int Major = 1;
            public const int Minor = 4;
            public const string Full = string.Format("{0}.{1}", Major, Minor);
        }

        public struct Administrator
        {
            public const string Name = "Queue Administrator";
            public const string Description = "";
            public const int Build = 5;
            public const string Version = string.Format("{0}.{1}", Product.Version.Full, Build);
            public const string Guid = "3708e4b1-7ce8-4100-9b22-bf6e83093823";
        }
    }
}