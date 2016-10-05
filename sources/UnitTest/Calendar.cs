using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Globalization;

namespace Queue.UnitTest
{
    [TestClass]
    public class Calendar
    {
        [TestMethod]
        public void WeekNumber()
        {
            var target = DateTime.Now;
            var formatInfo = DateTimeFormatInfo.CurrentInfo;
            var calendar = formatInfo.Calendar;
            int week = calendar.GetWeekOfYear(target, CalendarWeekRule.FirstFourDayWeek, formatInfo.FirstDayOfWeek);
            Assert.AreEqual(week, 40);
        }
    }
}