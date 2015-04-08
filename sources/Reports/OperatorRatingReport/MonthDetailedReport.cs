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
    internal class MonthDetailedReport : BaseDetailedReport<OperatorMonthRating>
    {
        public MonthDetailedReport(OperatorRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override void RenderData(ISheet worksheet, OperatorMonthRating[] data)
        {
            worksheet.SetColumnHidden(2, true);
            worksheet.SetColumnHidden(3, true);

            YearReportDataItem[] items = data.GroupBy(y => y.Year)
                                                .Select(y => new YearReportDataItem()
                                                {
                                                    Year = y.Key,
                                                    Months = y.GroupBy(m => m.Month)
                                                                    .Select(m => new MonthReportDataItem()
                                                                    {
                                                                        Month = m.Key,
                                                                        Ratings = GetOperatorsRatings(m.ToArray())
                                                                    })
                                                                    .OrderBy(m => m.Month)
                                                                    .ToArray()
                                                })
                                                .OrderBy(y => y.Year)
                                                .ToArray();

            int rowIndex = worksheet.LastRowNum + 1;

            foreach (YearReportDataItem item in items)
            {
                WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(item.Year));

                foreach (MonthReportDataItem month in item.Months)
                {
                    WriteBoldCell(worksheet.CreateRow(rowIndex++), 1, c => c.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Month)));

                    foreach (OperatorRating rating in month.Ratings)
                    {
                        IRow row = worksheet.CreateRow(rowIndex++);
                        WriteBoldCell(row, 4, c => c.SetCellValue(rating.Operator.ToString()));
                        RenderRating(row, rating);
                    }
                }
            }
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfMonth(settings.StartYear, settings.StartMonth);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfMonth(settings.FinishYear, settings.FinishMonth);
        }

        protected override ProjectionList GetProjections()
        {
            return GetCommonProjections()
                .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year")
                .Add(Projections.GroupProperty(Projections.SqlFunction("month", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Month");
        }

        private class YearReportDataItem
        {
            public int Year;
            public MonthReportDataItem[] Months;
        }

        private class MonthReportDataItem
        {
            public int Month;
            public OperatorRating[] Ratings;
        }
    }
}