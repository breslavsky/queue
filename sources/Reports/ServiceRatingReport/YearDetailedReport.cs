﻿using NHibernate;
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
        public YearDetailedReport(Guid[] servicesIds, ServiceRatingReportSettings settings)
            : base(servicesIds, settings)
        {
        }

        protected override void RenderData(ISheet worksheet, ServiceYearRating[] data)
        {
            worksheet.SetColumnHidden(1, true);
            worksheet.SetColumnHidden(2, true);
            worksheet.SetColumnHidden(3, true);

            ReportDataItem[] items = data.GroupBy(y => y.Year)
                                           .Select(y => new ReportDataItem()
                                           {
                                               Year = y.Key,
                                               Ratings = y.ToArray()
                                           })
                                           .OrderBy(y => y.Year)
                                           .ToArray();

            int rowIndex = worksheet.LastRowNum + 1;

            ServiceGroupDto root = GetServicesHierarchy();

            foreach (ReportDataItem item in items)
            {
                WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(item.Year));
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