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
    internal class YearDetailedReport : BaseDetailedReport<AdditionalServiceYearRating>
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
            AdditionalServiceYearRating dto = null;
            ClientRequest request = null;
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("YEAR", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Year));
        }

        protected override void RenderData(ISession session, ISheet worksheet, IList<AdditionalServiceYearRating> data)
        {
            worksheet.SetColumnHidden(1, true);
            worksheet.SetColumnHidden(2, true);

            var items = data.GroupBy(y => y.Year)
                                            .Select(y => new ReportDataItem()
                                            {
                                                Year = y.Key,
                                                Rating = y.ToArray()
                                            })
                                            .OrderBy(y => y.Year)
                                            .ToArray();

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (var item in items)
            {
                WriteCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(item.Year), styles[StandardCellStyles.BoldStyle]);

                foreach (var service in GetAdditionalServices(session))
                {
                    var row = worksheet.CreateRow(rowIndex++);
                    WriteCell(row, 3, c => c.SetCellValue(service.ToString()), styles[StandardCellStyles.BoldStyle]);

                    RenderServiceRating(row, service, item.Rating);
                }
            }
        }

        private class ReportDataItem
        {
            public int Year;

            public AdditionalServiceYearRating[] Rating;
        }
    }
}