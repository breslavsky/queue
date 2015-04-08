using System;
using System.Globalization;

namespace Queue.Common
{
    public class DayOfWeekTranslater : ITranslater
    {
        public string GetString(object key, params object[] parameters)
        {
            return DateTimeFormatInfo.CurrentInfo.GetDayName((DayOfWeek)key);
        }
    }
}