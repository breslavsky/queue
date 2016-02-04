using NHibernate;
using NHibernate.Criterion;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model.Common;
using System;
using System.Linq;

namespace Queue.Reports.ServiceRatingReport
{
    internal class YearDetailedReport : BaseDetailedReport<ServiceYearRating>
    {
        public YearDetailedReport(ServiceRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override void RenderData(ISheet worksheet, ServiceYearRating[] data)
        {
            worksheet.SetColumnHidden(1, true);
            worksheet.SetColumnHidden(2, true);
            worksheet.SetColumnHidden(3, true);

            var items = data.GroupBy(y => y.Year)
                                           .Select(y => new ReportDataItem()
                                           {
                                               Year = y.Key,
                                               Ratings = y.ToArray()
                                           })
                                           .OrderBy(y => y.Year)
                                           .ToArray();

            var rowIndex = worksheet.LastRowNum + 1;
            var root = GetServicesHierarchy();

            foreach (var item in items)
            {
                WriteCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(item.Year), styles[StandardCellStyles.BoldStyle]);
                WriteServicesRatings(worksheet, item.Ratings, root, ref  rowIndex);
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
            public ServiceRating[] Ratings;
        }
    }
}