using System;

namespace Queue.Common
{
    //TODO: переделать как расширение
    public static class DateTimeUtils
    {
        public static DateTime BeginOfYear(int year)
        {
            return new DateTime(year, 1, 1);
        }

        public static DateTime EndOfYear(int year)
        {
            return BeginOfYear(year)
                .AddYears(1)
                .AddSeconds(-1);
        }

        public static DateTime BeginOfMonth(int year, int month)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime EndOfMonth(int year, int month)
        {
            return BeginOfMonth(year, month)
                .AddMonths(1)
                .AddSeconds(-1);
        }

        public static DateTime BeginOfDay(int year, int month, int day)
        {
            return new DateTime(year, month, day);
        }

        public static DateTime EndOfDay(int year, int month, int day)
        {
            return BeginOfDay(year, month, day)
                .AddDays(1)
                .AddSeconds(-1);
        }
    }
}