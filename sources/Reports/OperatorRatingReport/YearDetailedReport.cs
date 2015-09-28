using NHibernate;
using NHibernate.Criterion;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model.Common;
using System;
using System.Linq;

namespace Queue.Reports.OperatorRatingReport
{
    internal class YearDetailedReport : BaseDetailedReport<OperatorYearRating>
    {
        public YearDetailedReport(OperatorRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override void RenderData(ISheet worksheet, OperatorYearRating[] data)
        {
            worksheet.SetColumnHidden(1, true);
            worksheet.SetColumnHidden(2, true);
            worksheet.SetColumnHidden(3, true);

           var items = data.GroupBy(y => y.Year)
                            .Select(y => new ReportDataItem()
                                            {
                                                Year = y.Key,
                                                Ratings = GetOperatorsRatings(y.ToArray())
                                            })
                            .OrderBy(y => y.Year)
                            .ToArray();

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (var item in items)
            {
                WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(item.Year));

                foreach (var rating in item.Ratings)
                {
                    var row = worksheet.CreateRow(rowIndex++);
                    WriteBoldCell(row, 4, c => c.SetCellValue(rating.Operator.ToString()));
                    RenderRating(row, rating);
                }
            }
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfYear(settings.StartYear);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfYear(settings.FinishYear);
        }

        protected override ProjectionList GetProjections()
        {
            return GetCommonProjections()
                    .Add(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32, Projections.Property("RequestDate"))), "Year");
        }

        private class ReportDataItem
        {
            public int Year;
            public OperatorRating[] Ratings;
        }
    }
}