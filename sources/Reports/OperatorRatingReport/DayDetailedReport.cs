using NHibernate;
using NHibernate.Criterion;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model.Common;
using System;
using System.Globalization;
using System.Linq;

namespace Queue.Reports.OperatorRatingReport
{
    internal class DayDetailedReport : BaseDetailedReport<OperatorDayRating>
    {
        public DayDetailedReport(Guid[] operators, OperatorRatingReportSettings settings)
            : base(operators, settings)
        {
        }

        protected override void RenderData(ISheet worksheet, OperatorDayRating[] data)
        {
            YearReportDataItem[] items = GetItems(data);

            worksheet.SetColumnHidden(3, true);

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (YearReportDataItem item in items)
            {
                WriteYearData(worksheet, item, ref rowIndex);
            }
        }

        private YearReportDataItem[] GetItems(OperatorDayRating[] data)
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
                                                        Ratings = GetOperatorsRatings(d.ToArray())
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

        private void WriteYearData(ISheet worksheet, YearReportDataItem data, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(data.Year));

            foreach (MonthReportDataItem month in data.Months)
            {
                WriteMonthData(worksheet, month, ref rowIndex);
            }
        }

        private void WriteMonthData(ISheet worksheet, MonthReportDataItem data, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 1, c =>
                            c.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(data.Month)));

            foreach (DayReportDataItem day in data.Days)
            {
                WriteDayData(worksheet, day, ref rowIndex);
            }
        }

        private void WriteDayData(ISheet worksheet, DayReportDataItem data, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 2, c => c.SetCellValue(data.Day));

            foreach (OperatorRating rating in data.Ratings)
            {
                IRow row = worksheet.CreateRow(rowIndex++);
                WriteBoldCell(row, 4, c => c.SetCellValue(rating.Operator.ToString()));
                RenderRating(row, rating);
            }
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
            public OperatorRating[] Ratings;
        }
    }
}