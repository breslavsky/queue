using System;

namespace Queue.Common
{
    public class ServerDateTime
    {
        private static class NestedServerTime
        {
            internal static readonly ServerDateTime serverDateTime = new ServerDateTime();
        }

        private static ServerDateTime serverDateTime
        {
            get { return NestedServerTime.serverDateTime; }
        }

        private TimeSpan shift;

        public static DateTime Now
        {
            get { return DateTime.Now - serverDateTime.shift; }
        }

        public static DateTime Today
        {
            get { return Now.Date; }
        }

        private ServerDateTime()
        {
            shift = TimeSpan.Zero;
        }

        public static void Sync(DateTime dateTime)
        {
            serverDateTime.shift = DateTime.Now - dateTime;
        }
    }
}