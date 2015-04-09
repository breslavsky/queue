using NHibernate;
using NHibernate.Criterion;
using NHibernate.Criterion.Lambda;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Queue.Reports.AdditionalServicesRatingReport
{
    internal class YearDetailedReport : BaseDetailedReport<YearAdditionalServiceRating>
    {
        public YearDetailedReport(AdditionalServicesRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfYear(settings.StartYear);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfYear(settings.FinishYear);
        }

        protected override void ModifyProjections(QueryOverProjectionBuilder<ClientRequestAdditionalService> builder)
        {
            YearAdditionalServiceRating dto = null;
            ClientRequest request = null;
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("YEAR", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Year));
        }

        protected override void RenderData(ISession session, ISheet worksheet, IList<YearAdditionalServiceRating> data)
        {
            worksheet.SetColumnHidden(1, true);
            worksheet.SetColumnHidden(2, true);

            WriteOperatorsHeader(worksheet, session);

            ReportDataItem[] items = data.GroupBy(y => y.Year)
                                            .Select(y => new ReportDataItem()
                                            {
                                                Year = y.Key,
                                                Rating = y.ToArray()
                                            })
                                            .OrderBy(y => y.Year)
                                            .ToArray();

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (ReportDataItem item in items)
            {
                WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(item.Year));

                foreach (AdditionalService service in GetAdditionalServices(session))
                {
                    IRow row = worksheet.CreateRow(rowIndex++);
                    WriteBoldCell(row, 3, c => c.SetCellValue(service.ToString()));

                    RenderServiceRating(row, service, item.Rating);
                }
            }
        }

        private class ReportDataItem
        {
            public int Year;

            public YearAdditionalServiceRating[] Rating;
        }
    }
}