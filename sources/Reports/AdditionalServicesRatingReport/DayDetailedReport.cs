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
    internal class DayDetailedReport : BaseDetailedReport<AdditionalServiceDayRating>
    {
        public DayDetailedReport(AdditionalServicesRatingReportSettings settings)
            : base(settings)
        {
        }

        protected override DateTime GetStartDate()
        {
            return DateTimeUtils.BeginOfDay(settings.StartYear, settings.StartMonth, settings.StartDay);
        }

        protected override DateTime GetFinishDate()
        {
            return DateTimeUtils.EndOfDay(settings.FinishYear, settings.FinishMonth, settings.FinishDay);
        }

        protected override void ModifyProjections(QueryOverProjectionBuilder<ClientRequestAdditionalService> builder)
        {
            AdditionalServiceDayRating dto = null;
            ClientRequest request = null;
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("YEAR", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Year));
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("MONTH", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Month));
            builder.Select(Projections.GroupProperty(
                Projections.SqlFunction("DAY", NHibernateUtil.Date, Projections.Property(() => request.RequestDate))).WithAlias(() => dto.Day));
        }

        protected override void RenderData(ISession session, ISheet worksheet, IList<AdditionalServiceDayRating> data)
        {
            var items = GetItems(data);

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (var item in items)
            {
                WriteYearData(session, worksheet, item, ref rowIndex);
            }
        }

        private YearReportDataItem[] GetItems(IList<AdditionalServiceDayRating> data)
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

        private void WriteYearData(ISession session, ISheet worksheet, YearReportDataItem data, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 0, c => c.SetCellValue(data.Year));

            foreach (var month in data.Months)
            {
                WriteMonthData(session, worksheet, month, ref rowIndex);
            }
        }

        private void WriteMonthData(ISession session, ISheet worksheet, MonthReportDataItem data, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 1, c =>
                            c.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(data.Month)));

            foreach (var day in data.Days)
            {
                WriteDayData(session, worksheet, day, ref rowIndex);
            }
        }

        private void WriteDayData(ISession session, ISheet worksheet, DayReportDataItem item, ref int rowIndex)
        {
            WriteBoldCell(worksheet.CreateRow(rowIndex++), 2, c => c.SetCellValue(item.Day));

            foreach (var service in GetAdditionalServices(session))
            {
                var row = worksheet.CreateRow(rowIndex++);
                WriteBoldCell(row, 3, c => c.SetCellValue(service.ToString()));

                RenderServiceRating(row, service, item.Ratings);
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
            public DayReportDataItem[] Days;
        }

        private class DayReportDataItem
        {
            public int Day;
            public AdditionalServiceYearRating[] Ratings;
        }
    }
}