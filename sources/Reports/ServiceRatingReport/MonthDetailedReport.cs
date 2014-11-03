using NHibernate;
using NHibernate.Criterion;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Queue.Common;
using Queue.Model;
using Queue.Model.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Translation = Queue.Model.Common.Translation;

namespace Queue.Reports.ServiceRatingReport
{
    internal class MonthDetailedReport : BaseDetailedReport<ServiceMonthRating>
    {
        public MonthDetailedReport(Guid[] services, ServiceRatingReportSettings settings)
            : base(services, settings)
        {
        }

        public override HSSFWorkbook InternalGenerate(ISession session, ServiceDayRating[] data)
        {
            HSSFWorkbook workbook = new HSSFWorkbook(new MemoryStream(Templates.ServiceRating));
            ISheet worksheet = workbook.GetSheetAt(0);

            IDataFormat format = workbook.CreateDataFormat();
            ICellStyle boldCellStyle = CreateCellBoldStyle(workbook);

            IRow row;
            ICell cell;
            int rowIndex = worksheet.LastRowNum + 1;

            var monthsData = new Dictionary<int, Dictionary<int, List<ServiceMonthRating>>>();
            foreach (var year in data.OrderBy(r => r.Year).GroupBy(r => r.Year))
            {
                if (!monthsData.ContainsKey(year.Key))
                {
                    monthsData[year.Key] = new Dictionary<int, List<ServiceMonthRating>>();
                }

                var months = monthsData[year.Key];

                foreach (var month in year.OrderBy(r => r.Month).GroupBy(r => r.Month))
                {
                    if (!months.ContainsKey(month.Key))
                    {
                        months[month.Key] = new List<ServiceMonthRating>();
                    }

                    var ratings = months[month.Key];
                    foreach (var rating in month)
                    {
                        ratings.Add(rating);
                    }
                }
            }

            #region create header

            row = worksheet.CreateRow(0);
            cell = row.CreateCell(0);
            cell.SetCellValue(string.Format("Период с {0} по {1}", GetStartDate().ToShortDateString(), GetStartDate().ToShortDateString()));
            cell.CellStyle = boldCellStyle;

            worksheet.SetColumnHidden(2, true);
            worksheet.SetColumnHidden(3, true);

            #endregion create header

            #region render data

            foreach (var year in monthsData)
            {
                row = worksheet.CreateRow(rowIndex++);
                cell = row.CreateCell(0);
                cell.SetCellValue(year.Key);
                cell.CellStyle = boldCellStyle;

                foreach (var month in year.Value)
                {
                    var ratings = month.Value;

                    row = worksheet.CreateRow(rowIndex++);
                    cell = row.CreateCell(1);
                    cell.SetCellValue(CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month.Key));
                    cell.CellStyle = boldCellStyle;

                    Action<ServiceGroup> recursion = null;
                    recursion = (parentGroup) =>
                    {
                        var serviceGroups = session.CreateCriteria<ServiceGroup>()
                            .AddOrder(Order.Asc("SortId"))
                            .Add(parentGroup != null
                                ? Restrictions.Eq("ParentGroup", parentGroup)
                                : Restrictions.IsNull("ParentGroup"))
                           .List<ServiceGroup>();

                        foreach (var g in serviceGroups)
                        {
                            row = worksheet.CreateRow(rowIndex++);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(g.ToString());
                            cell.CellStyle = boldCellStyle;

                            recursion(g);

                            var services = session.CreateCriteria<Service>()
                                .Add(Restrictions.Eq("ServiceGroup", g))
                                .AddOrder(Order.Asc("SortId"))
                                .List<Service>();

                            var hasServices = false;

                            foreach (var service in services)
                            {
                                if (servicesIds.Count() > 0 && !servicesIds.Any(i => i.Equals(service.Id)))
                                {
                                    continue;
                                }

                                hasServices = true;

                                row = worksheet.CreateRow(rowIndex++);
                                cell = row.CreateCell(4);
                                cell.SetCellValue(service.ToString());

                                if (service.Type != ServiceType.None && settings.IsServiceTypes)
                                {
                                    cell.CellStyle = boldCellStyle;

                                    var translation = Translation.ServiceType.ResourceManager;

                                    foreach (ServiceType serviceType in Enum.GetValues(typeof(ServiceType)))
                                    {
                                        if (service.Type.HasFlag(serviceType))
                                        {
                                            row = worksheet.CreateRow(rowIndex++);
                                            cell = row.CreateCell(4);
                                            cell.SetCellValue(translation.GetString(serviceType.ToString()));

                                            var rating = ratings.FirstOrDefault(r => r.Service.Equals(service)
                                                && r.ServiceType.Equals(serviceType));
                                            renderRating(row, rating != null ? rating : new ServiceDayRating());
                                        }
                                    }
                                }
                                else
                                {
                                    var rating = ratings.FirstOrDefault(r => r.Service.Equals(service));
                                    renderRating(row, rating != null ? rating : new ServiceDayRating());
                                }
                            }

                            if (!hasServices)
                            {
                                worksheet.RemoveRow(row);
                                rowIndex--;
                            }
                        }
                    };

                    recursion(null);
                }

            #endregion render data
            }
            return workbook;
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
    }
}