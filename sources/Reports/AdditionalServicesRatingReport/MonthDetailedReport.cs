using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    internal class MonthDetailedReport : BaseDetailedReport<AdditionalServiceMonthRating>
    {
        public MonthDetailedReport(AdditionalServicesRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfMonth(settings.StartYear, settings.StartMonth);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfMonth(settings.FinishYear, settings.FinishMonth);
        }

        protected override void ModifyProjections(QueryOverProjectionBuilder<ClientRequestAdditionalService> builder)
        {
            AdditionalServiceMonthRating dto = null;
            ClientRequest request = null;
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("YEAR", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Year));
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("MONTH", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Month));
        }

        protected override void RenderData(ISession session, ISheet worksheet, IList<AdditionalServiceMonthRating> data)
        {
            worksheet.SetColumnHidden(2, true);

            YearReportDataItem[] items = data.GroupBy(y => y.Year)
                                               .Select(y => new YearReportDataItem()
                                               {
                                                   Year = y.Key,
                                                   Months = y.GroupBy(m => m.Month)
                                                                   .Select(m => new MonthReportDataItem()
                                                                   {
                                                                       Month = m.Key,
                                                                       Ratings = m.ToArray()
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

                    foreach (AdditionalService service in GetAdditionalServices(session))
                    {
                        IRow row = worksheet.CreateRow(rowIndex++);
                        WriteBoldCell(row, 3, c => c.SetCellValue(service.ToString()));

                        RenderServiceRating(row, service, month.Ratings);
                    }
                }
            }
        }

        private class YearReportDataItem
        {
            public int Year;
            public MonthReportDataItem[] Months;
        }

        private class MonthReportDataItem
        {
            public int Month;
            public AdditionalServiceMonthRating[] Ratings;
        }
    }
}