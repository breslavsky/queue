using NHibernate;
using NHibernate.Criterion;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model.Common;
using System;
using System.Globalization;
using System.Linq;

namespace Queue.Reports.ServiceRatingReport
{
    internal class DayDetailedReport : BaseDetailedReport<ServiceDayRating>
    {
        public DayDetailedReport(ServiceRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override void RenderData(ISheet worksheet, ServiceDayRating[] data)
        {
            var items = GetItems(data);

            worksheet.SetColumnHidden(3, true);

            var root = GetServicesHierarchy();

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (var year in items)
            {
                WriteCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(year.Year), styles[StandardCellStyles.BoldStyle]);

                foreach (var month in year.Months)
                {
                    WriteCell(worksheet.CreateRow(rowIndex++), 1,
                        c => c.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Month)),
                                styles[StandardCellStyles.BoldStyle]);

                    foreach (var day in month.Days)
                    {
                        WriteCell(worksheet.CreateRow(rowIndex++), 2, c => c.SetCellValue(day.Day), styles[StandardCellStyles.BoldStyle]);
                        WriteServicesRatings(worksheet, day.Ratings, root, ref  rowIndex);
                    }
                }
            }
        }

        private YearReportDataItem[] GetItems(ServiceDayRating[] data)
        {
            return data.GroupBy(y => y.Year)
                        .Select(y => new YearReportDataItem()
                        {
                            Year = y.Key,
                            Months = y.GroupBy(m => m.Month)
                                        .Select(m => new MonthReportDataItem()
                                        {
                                            Month = m.Key,
                                            Days = m.GroupBy(d => d.Day)
                                                    .Select(d => new DayReportDataItem()
                                                    {
                                                        Day = d.Key,
                                                        Ratings = d.ToArray()
                                                    })
                                                    .OrderBy(d => d.Day)
                                                    .ToArray()
                                        })
                                        .OrderBy(m => m.Month)
                                        .ToArray()
                        })
                        .OrderBy(y => y.Year)
                        .ToArray();
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfDay(settings.StartYear, settings.StartMonth, settings.StartDay);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfDay(settings.FinishYear, settings.FinishMonth, settings.FinishDay);
        }

        protected override ProjectionList GetProjections()
        {
            return GetCommonProjections()
                            .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year")
                            .Add(Projections.GroupProperty(Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Month")
                            .Add(Projections.GroupProperty(Projections.SqlFunction("day", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Day");
        }

        private class YearReportDataItem
        {
            public int Year;
            public MonthReportDataItem[] Months;
        }

        private class MonthReportDataItem
        {
            public int Month;
            public DayReportDataItem[] Days;
        }

        private class DayReportDataItem
        {
            public int Day;
            public ServiceRating[] Ratings;
        }
    }
}